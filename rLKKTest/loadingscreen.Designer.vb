<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class loadingscreen
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Label2 = New Label()
        progress = New Label()
        status = New Label()
        SuspendLayout()
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.FlatStyle = FlatStyle.System
        Label2.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        Label2.Location = New Point(12, 26)
        Label2.Name = "Label2"
        Label2.Size = New Size(79, 24)
        Label2.TabIndex = 1
        Label2.Text = "Loading ..."
        Label2.UseCompatibleTextRendering = True
        ' 
        ' progress
        ' 
        progress.AutoSize = True
        progress.FlatStyle = FlatStyle.System
        progress.Font = New Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point)
        progress.Location = New Point(122, 17)
        progress.Name = "progress"
        progress.Size = New Size(47, 35)
        progress.TabIndex = 2
        progress.Text = "0%"
        progress.UseCompatibleTextRendering = True
        ' 
        ' status
        ' 
        status.FlatStyle = FlatStyle.System
        status.Font = New Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point)
        status.Location = New Point(12, 50)
        status.Name = "status"
        status.Size = New Size(248, 24)
        status.TabIndex = 2
        status.Text = "Loading ..."
        status.TextAlign = ContentAlignment.MiddleCenter
        status.UseCompatibleTextRendering = True
        ' 
        ' loadingscreen
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.OliveDrab
        ClientSize = New Size(260, 72)
        Controls.Add(status)
        Controls.Add(progress)
        Controls.Add(Label2)
        ForeColor = Color.White
        FormBorderStyle = FormBorderStyle.None
        Name = "loadingscreen"
        Opacity = 0.8R
        StartPosition = FormStartPosition.CenterParent
        Text = "loadingscreen"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label2 As Label
    Friend WithEvents progress As Label
    Friend WithEvents status As Label
End Class
