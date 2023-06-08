using DocumentValidator;

Console.WriteLine(Validator.ValidateDocumentNumber(DocumentType.DNI, "12345678912")); //false
Console.WriteLine(Validator.ValidateDocumentNumber(DocumentType.Passport, "aa1234567")); //false
Console.WriteLine(Validator.ValidateDocumentNumber(DocumentType.RNC, "123456789")); // false