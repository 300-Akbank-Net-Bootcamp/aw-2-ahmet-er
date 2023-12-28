

1 - migration olusturma
Dbcontext in oldugu projede
	dotnet ef migration addUniqueMigrationName -s ../Vb.Api/

2 - migration degisikliklerini db ye gecme yansitma guncelle migrate etme
Olusan migrationlarin calistirilmasi
sln dizininde
	dotnet ef database update --project "./Vb.Data" --startup-project "./Vb.Api"

dummy data for customer:
	{
  "id": 1,
  "insertUserId": 1,
  "insertDate": "2023-12-24T20:50:21.083Z",
  "updateUserId": 1,
  "updateDate": "2023-12-24T20:50:21.083Z",
  "isActive": true,
  "identityNumber": "12345678901",
  "firstName": "John",
  "lastName": "Doe",
  "customerNumber": 1001,
  "dateOfBirth": "1990-01-01T00:00:00Z",
  "lastActivityDate": "2023-12-24T20:50:21.083Z",
  "addresses": [
    {
      "id": 1,
      "insertUserId": 1,
      "insertDate": "2023-12-24T20:50:21.083Z",
      "updateUserId": 1,
      "updateDate": "2023-12-24T20:50:21.083Z",
      "isActive": true,
      "customerId": 1,
      "customer": "John Doe",
      "address1": "123 Main St",
      "address2": "Apt 4",
      "country": "USA",
      "city": "New York",
      "county": "Manhattan",
      "postalCode": "10001",
      "isDefault": true
    }
  ],
  "contacts": [
    {
      "id": 1,
      "insertUserId": 1,
      "insertDate": "2023-12-24T20:50:21.083Z",
      "updateUserId": 1,
      "updateDate": "2023-12-24T20:50:21.083Z",
      "isActive": true,
      "customerId": 1,
      "customer": "John Doe",
      "contactType": "Email",
      "information": "john.doe@example.com",
      "isDefault": true
    }
  ],
  "accounts": [
    {
      "id": 1,
      "insertUserId": 1,
      "insertDate": "2023-12-24T20:50:21.083Z",
      "updateUserId": 1,
      "updateDate": "2023-12-24T20:50:21.083Z",
      "isActive": true,
      "customerId": 1,
      "customer": "John Doe",
      "accountNumber": 10001,
      "iban": "US12345678901234567890",
      "balance": 5000,
      "currencyType": "USD",
      "name": "Personal Account",
      "openDate": "2023-12-24T20:50:21.083Z",
      "accountTransactions": [
        {
          "id": 1,
          "insertUserId": 1,
          "insertDate": "2023-12-24T20:50:21.083Z",
          "updateUserId": 1,
          "updateDate": "2023-12-24T20:50:21.083Z",
          "isActive": true,
          "accountId": 1,
          "account": "Personal Account",
          "referenceNumber": "TXN12345",
          "transactionDate": "2023-12-24T20:50:21.083Z",
          "amount": 1000,
          "description": "Deposit",
          "transferType": "Deposit"
        }
      ],
      "eftTransactions": [
        {
          "id": 1,
          "insertUserId": 1,
          "insertDate": "2023-12-24T20:50:21.083Z",
          "updateUserId": 1,
          "updateDate": "2023-12-24T20:50:21.083Z",
          "isActive": true,
          "accountId": 1,
          "account": "Personal Account",
          "referenceNumber": "EFT56789",
          "transactionDate": "2023-12-24T20:50:21.083Z",
          "amount": 500,
          "description": "Online Transfer",
          "senderAccount": "Sender Account",
          "senderIban": "SENDERIBAN12345",
          "senderName": "Sender Name"
        }
      ]
    }
  ]
}