Imports LoginLibrary.DataClasses

Public Class MainForm

    Private userNameBytes As Byte()
    Private userPasswordBytes As Byte()

    Private ProductBindingSource As New BindingSource

    Public Sub New(pNameBytes As Byte(), pPasswordBytes As Byte())

        InitializeComponent()

        userNameBytes = pNameBytes
        userPasswordBytes = pPasswordBytes

    End Sub
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim ops As New DataOperations(userNameBytes, userPasswordBytes, "KARENS-PC", "UserLoginExample")

        Dim productTable = ops.ReadProductsByCategory(1)
        If ops.IsSuccessFul Then
            ProductBindingSource.DataSource = productTable
            ProductsDataGridView.DataSource = ProductBindingSource
        Else
            MessageBox.Show($"Encountered issues: {ops.LastExceptionMessage}")
        End If

    End Sub
    Private Sub MainFormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.ExitThread()
    End Sub
End Class