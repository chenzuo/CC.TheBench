Set-ExecutionPolicy RemoteSigned

Drop a .publishsettings file in here
Run Import-AzurePublishSettingsFile

When behind a proxy:
[System.Net.WebRequest]::DefaultWebProxy.Credentials = [System.Net.CredentialCache]::DefaultCredentials