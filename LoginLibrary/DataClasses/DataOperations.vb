Imports System.Data.SqlClient
Imports LoginLibrary.SecurityClasses
Imports SupportLibrary

Namespace DataClasses

    Public Class DataOperations
        Inherits BaseExceptionProperties

        Private ConnectionString As String
        '
        Public Sub New(
            pNameBytes As Byte(),
            pPasswordBytes As Byte(),
            pServerName As String,
            pCatalogName As String)

            Dim secureOperations = New Encryption

            ConnectionString = $"Data Source={pServerName};Initial Catalog={pCatalogName};" &
                               $"User Id={secureOperations.Decrypt(pNameBytes, "111")};" &
                               $"Password={secureOperations.Decrypt(pPasswordBytes, "111")};" &
                               "Integrated Security=False"

            Console.WriteLine()
        End Sub
        ''' <summary>
        ''' Connect to database via validated user name and password passed in the
        ''' new constructor.
        ''' 
        ''' There are still failure points which include permissions to the tables
        ''' for the user.
        ''' </summary>
        ''' <param name="pCategoryIdentifier"></param>
        ''' <returns></returns>
        Public Function ReadProductsByCategory(pCategoryIdentifier As Integer) As DataTable

            mHasException = False

            Dim productDataTable As New DataTable

            Dim selectStatement =
                    <SQL>
                    SELECT P.ProductID ,
                           P.ProductName ,
                           P.SupplierID ,
                           P.CategoryID ,
                           P.QuantityPerUnit ,
                           P.UnitPrice ,
                           P.UnitsInStock ,
                           S.CompanyName AS Supplier
                    FROM   dbo.Products AS P
                           INNER JOIN dbo.Categories AS C ON P.CategoryID = C.CategoryID
                           INNER JOIN dbo.Suppliers AS S ON P.SupplierID = S.SupplierID
                    WHERE  ( P.CategoryID = @CategoryID );
                    </SQL>.Value


            Using cn As New SqlConnection With {.ConnectionString = ConnectionString}
                Using cmd As New SqlCommand With {.Connection = cn}

                    cmd.Parameters.AddWithValue("@CategoryID", pCategoryIdentifier)
                    cmd.CommandText = selectStatement

                    Try
                        cn.Open()
                        productDataTable.Load(cmd.ExecuteReader())

                        Dim identifiers = productDataTable.
                                Columns.Cast(Of DataColumn).
                                Where(Function(column) column.ColumnName.EndsWith("ID")).
                                Select(Function(item) item.ColumnName).ToList()

                        For Each columnName As String In identifiers
                            productDataTable.Columns(columnName).ColumnMapping = MappingType.Hidden
                        Next

                    Catch ex As Exception
                        mHasException = True
                        mLastException = ex
                    End Try
                End Using
            End Using

            Return productDataTable

        End Function
    End Class
End Namespace