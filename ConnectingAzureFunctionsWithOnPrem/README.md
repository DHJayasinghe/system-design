

1. az group create --name azure-relay-functionapp-test --location australiaeast
2. az appservice plan create --name azure-relay-functionapp-plan --resource-group azure-relay-functionapp-test --sku B1 --location australiaeast
3. az storage account create --name azrelayfuncapptest --resource-group azure-relay-functionapp-test --location australiaeast --sku Standard_LRS
4. az functionapp create --name azure-relay-functionapp-test --resource-group azure-relay-functionapp-test --plan azure-relay-functionapp-plan --runtime dotnet --os-type Windows --functions-version 4 --storage-account azrelayfuncapptest
5. az functionapp create --name azure-relay-functionapp-test2 --resource-group azure-relay-functionapp-test --plan azure-relay-functionapp-plan --runtime dotnet --os-type Windows --functions-version 4 --storage-account azrelayfuncapptest