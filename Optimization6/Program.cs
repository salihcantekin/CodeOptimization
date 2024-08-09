using System.Text.Json;


var transactionFileProcessor = new TransactionFileProcessor();
var azureService = new AzureService();

void TransactionCreated(Transaction transaction)
{
    try
    {
        azureService.SendTransaction(transaction);
    }
    catch
    {
        transactionFileProcessor.SaveToFile(transaction);
    }
}




void ReSendPendingTransactions()
{
    var pendingTransactions = transactionFileProcessor.GetPendingTransactions();

    foreach (var tran in pendingTransactions)
    {
        azureService.SendTransaction(tran);
    }
}


class AzureService
{
    public void SendTransaction(Transaction transaction)
    {
        PostTransaction(transaction);
    }

    private HttpResponseMessage PostTransaction(Transaction transaction)
    {
        var randomNumber = Random.Shared.Next(2);

        if (randomNumber == 0)
        {
            throw new Exception("50% change exception");
        }

        return null;
    }
}

class TransactionFileProcessor
{
    public void SaveToFile(Transaction transaction)
    {
        var json = JsonSerializer.Serialize(transaction);

        var fileName = $"{transaction.Id}.tran";

        File.WriteAllText(fileName, json);
    }

    public List<Transaction> GetPendingTransactions()
    {
        var fileList = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.tran");

        return fileList
                .Select(i => JsonSerializer.Deserialize<Transaction>(i))
                .ToList();
    }

}


class Transaction(int Id, decimal TotalPrice)
{
    public int Id { get; } = Id;
    public decimal TotalPrice { get; } = TotalPrice;

    public byte SendCount { get; set; } = 0;
}
