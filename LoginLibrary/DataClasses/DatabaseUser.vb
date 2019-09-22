Imports System.Data.SqlClient
Imports LoginLibrary.SecurityClasses
Imports LoginLibrary.SupportClasses

Namespace DataClasses
    ''' <summary>
    ''' Responsible to validating a user has permissions to access the database, not tables.
    ''' </summary>
    Public Class DatabaseUser
        Private serverName As String
        Private catalogName As String
        Public Sub New(pServerName As String, pCatalogName As String)
            serverName = pServerName
            catalogName = pCatalogName
        End Sub
        Public Function Login(pNameBytes As Byte(), pPasswordBytes As Byte()) As SqlServerLoginResult
            Dim loginResult = New SqlServerLoginResult

            Dim secureOperations = New Encryption
            Dim userName = secureOperations.Decrypt(pNameBytes, "111")
            Dim userPassword = secureOperations.Decrypt(pPasswordBytes, "111")


            Dim ConnectionString As String =
                    $"Data Source={serverName};Initial Catalog={catalogName};User Id={userName};Password={userPassword};Integrated Security=False"

            Using cn As New SqlConnection With {.ConnectionString = ConnectionString}
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