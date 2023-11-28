using Example_1_Solution;




var customerRepo = new CustomerRepository();

var customer1 = new Customer { Id = 1, Name = "John", IsActive = true };
var customer2 = new Customer { Id = 2, Name = "Jane", IsActive = false };

customerRepo.Add(customer1);
customerRepo.Add(customer2);

var activeCustomers = customerRepo.GetAllActive();

var discount = DiscountCalculator.Calculate(customer1);