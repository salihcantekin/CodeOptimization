using Example_1;



var customerRepo = new CustomerRepository();

var customer1 = new Customer { Id = 1, Name = "Salih", IsActive = true };
var customer2 = new Customer { Id = 2, Name = "Cantekin", IsActive = false };

customerRepo.Add(customer1);
customerRepo.Add(customer2);





var allCustomers = customerRepo.GetAll();

var activeCustomers = allCustomers
                        .Where(c => c.IsActive)
                        .ToList();

var discount = customer1.CalculateDiscount();
