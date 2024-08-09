using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Optimization6.Exceptions;

namespace Optimization6;

public class Worker
{
    TransactionFileProcessor transactionFileProcessor = new();
    AzureService azureService = new();

    async Task TransactionCreated(Transaction transaction)
    {
        try
        {
            await azureService.SendTransaction(transaction);
            // Delete file
        }
        catch
        {
            transactionFileProcessor.SaveToFile(transaction);
        }
    }

    async Task ReSendPendingTransactions()
    {
        var pendingTransactions = await transactionFileProcessor.GetPendingTransactions();

        foreach (var transaction in pendingTransactions)
        {
            try
            {
                await azureService.SendTransaction(transaction);
                transactionFileProcessor.DeleteFile(transaction); // Başarılı gönderim sonrası dosyayı sil
            }
            catch (CustomNetworkException)
            {
                transaction.SendCount++;
                transactionFileProcessor.SaveToFile(transaction, isFailed: false);
            }
            catch (Exception)
            {
                // Gönderim sırasında bir hata oluşursa, burada bir işlem yapılabilir.
                // Örneğin, bu transaction nesnesini failedTransactionsPath'e taşıyabiliriz.
                transactionFileProcessor.SaveToFile(transaction, isFailed: true);
            }
        }
    }
}

class AzureService
{
    private readonly HttpClient httpClient;
    private readonly string azureEndpointUrl = "https://yourazurefunctionapp.azurewebsites.net/api/Transaction";

    public AzureService()
    {
        httpClient = new HttpClient();
    }

    public async Task SendTransaction(Transaction transaction)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(transaction);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(azureEndpointUrl, content);

            response.EnsureSuccessStatusCode(); // Bu, başarılı bir yanıt durum kodu olmadığında bir hata fırlatır.
        }
        catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            throw new CustomBadRequestException("Azure service returned BadRequest. Transaction might be invalid.");
        }
        catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable || e.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
        {
            throw new CustomNetworkException("Azure service is unavailable or timed out.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while sending transaction: {ex.Message}");
        }
    }
}


class TransactionFileProcessor
{
    private string pendingTransactionsPath = Path.Combine(Directory.GetCurrentDirectory(), "Transactions");
    private string failedTransactionsPath = Path.Combine(Directory.GetCurrentDirectory(), "FailedTransactions");

    public TransactionFileProcessor()
    {
        // Eğer klasörler yoksa oluştur
        if (!Directory.Exists(pendingTransactionsPath))
            Directory.CreateDirectory(pendingTransactionsPath);

        if (!Directory.Exists(failedTransactionsPath))
            Directory.CreateDirectory(failedTransactionsPath);
    }

    public void SaveToFile(Transaction transaction, bool isFailed = false)
    {
        string json;

        try
        {
            json = JsonSerializer.Serialize(transaction);
        }
        catch (NotSupportedException)
        {
            return;
        }

        var path = isFailed ? failedTransactionsPath : pendingTransactionsPath;
        var fileName = Path.Combine(path, $"{transaction.Id}.tran");

        File.WriteAllText(fileName, json);
    }

    public void DeleteFile(Transaction transaction)
    {
        var fileName = Path.Combine(pendingTransactionsPath, $"{transaction.Id}.tran");
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    public async Task<List<Transaction>> GetPendingTransactions()
    {
        var fileList = Directory.GetFiles(pendingTransactionsPath, "*.tran");

        var transactions = new List<Transaction>();
        foreach (var file in fileList)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var transaction = JsonSerializer.Deserialize<Transaction>(json);
                if (transaction != null)
                {
                    transactions.Add(transaction);
                }
            }
            catch (JsonException)
            {
                MoveFileToFailedDirectory(file);
            }
        }

        return transactions;
    }

    private void MoveFileToFailedDirectory(string sourceFilePath)
    {
        var fileName = Path.GetFileName(sourceFilePath);
        var destFilePath = Path.Combine(failedTransactionsPath, fileName);

        if (File.Exists(destFilePath))
        {
            File.Delete(destFilePath); // Hedefte aynı isimde dosya varsa sil
        }

        File.Move(sourceFilePath, destFilePath);
    }
}


