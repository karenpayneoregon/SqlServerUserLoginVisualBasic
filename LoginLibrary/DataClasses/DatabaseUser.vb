Imports System.Data.SqlClient
Imports System.Security
Imports LoginLibrary.SecurityClasses
Imports LoginLibrary.SupportClasses

Namespace DataClasses
    ''' <summary>
    ''' Responsible to validating a user has permissions 
    ''' to access the database, not tables.
    ''' </summary>
    Public Class DatabaseUser
        Private serverName As String
        Private catalogName As String
        Public Sub New(pServerName As String, pCatalogName As String)
            serverName = pServerName
            catalogName = pCatalogName
        End Sub
        ''' <summary>
        ''' Alternate method to login
        ''' SqlCredential Class
        ''' https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcredential?view=netframework-4.8
        ''' </summary>
        Public Function SqlCredentialLogin(pNameBytes As Byte(), pPasswordBytes As Byte()) As SqlServerLoginResult
            Dim loginResult = New SqlServerLoginResult
            Dim secureOperations = New Encryption

            Dim userName = secureOperations.Decrypt(pNameBytes, "111")
            Dim userPassword = secureOperations.Decrypt(pPasswordBytes, "111")

            Dim connectionString As String =
                    $"Data Source={serverName};" &
                    $"Initial Catalog={catalogName};"


            Dim securePassword = New SecureString()

            For Each character In userPassword
                securePassword.AppendChar(character)
            Next

            securePassword.MakeReadOnly()

            Dim credentials = New SqlCredential(userName, securePassword)

            Using cn = New SqlConnection With {.ConnectionString = connectionString}
                Try
                    cn.Credential = credentials
                    cn.Open()
                    loginResult.Success = True
                Catch failedLoginException As SqlException When failedLoginException.Number = 18456
                    loginResult.Success = False
                    loginResult.GenericException = False
                    loginResult.Message = "Can not access data."
                Catch genericSqlException As SqlException
                    loginResult.Success = False
                    loginResult.GenericException = False
                    loginResult.Message = "Can not access data."
                Catch ex As Exception
                    loginResult.Success = False
                    loginResult.GenericException = True
                    loginResult.Message = ex.Message
                End Try

            End Using

            Return loginResult

        End Function
        Public Function Login(pNameBytes As Byte(), pPasswordBytes As Byte()) As SqlServerLoginResult
            Dim loginResult = New SqlServerLoginResult

            Dim secureOperations = New Encryption
            Dim userName = secureOperations.Decrypt(pNameBytes, "111")
            Dim userPassword = secureOperations.Decrypt(pPasswordBytes, "111")

            Dim connectionString As String =
                    $"Data Source={serverName};" &
                    $"Initial Catalog={catalogName};" &
                    $"User Id={userName};Password={userPassword};" &
                    "Integrated Security=False"

            Using cn As New SqlConnection With {.ConnectionString = connectionString}
                Try
                    cn.Open()
                    loginResult.Success = True
                Catch failedLoginException As SqlException When failedLoginException.Number = 18456
                    loginResult.Success = False
                    loginResult.GenericException = False
                    loginResult.Message = "Can not access data."
                Catch genericSqlException As SqlException
                    loginResult.Success = False
                    loginResult.GenericException = False
                    loginResult.Message = "Can not access data."
                Catch ex As Exception
                    loginResult.Success = False
                    loginResult.GenericException = True
                    loginResult.Message = ex.Message
                End Try
            End Using

            Return loginResult

        End Function
    End Class
End Namespace