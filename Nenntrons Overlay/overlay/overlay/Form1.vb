Imports System.Runtime.InteropServices

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PictureBox1.AllowDrop = True

        Me.FormBorderStyle = FormBorderStyle.None
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Label1.Text = "Opacity"

        TopMost = True

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.FillRectangle(Brushes.Green, Top)
        e.Graphics.FillRectangle(Brushes.Green, Left)
        e.Graphics.FillRectangle(Brushes.Green, Right)
        e.Graphics.FillRectangle(Brushes.Green, Bottom)
    End Sub

    Private Const HTLEFT As Integer = 10, HTRIGHT As Integer = 11, HTTOP As Integer = 12, HTTOPLEFT As Integer = 13, HTTOPRIGHT As Integer = 14, HTBOTTOM As Integer = 15, HTBOTTOMLEFT As Integer = 16, HTBOTTOMRIGHT As Integer = 17

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If m.Msg = &H84 Then
            Dim mp = Me.PointToClient(Cursor.Position)

            If TopLeft.Contains(mp) Then
                m.Result = CType(HTTOPLEFT, IntPtr)
            ElseIf TopRight.Contains(mp) Then
                m.Result = CType(HTTOPRIGHT, IntPtr)
            ElseIf BottomLeft.Contains(mp) Then
                m.Result = CType(HTBOTTOMLEFT, IntPtr)
            ElseIf BottomRight.Contains(mp) Then
                m.Result = CType(HTBOTTOMRIGHT, IntPtr)
            ElseIf Top.Contains(mp) Then
                m.Result = CType(HTTOP, IntPtr)
            ElseIf Left.Contains(mp) Then
                m.Result = CType(HTLEFT, IntPtr)
            ElseIf Right.Contains(mp) Then
                m.Result = CType(HTRIGHT, IntPtr)
            ElseIf Bottom.Contains(mp) Then
                m.Result = CType(HTBOTTOM, IntPtr)
            End If
        End If
    End Sub

    Dim rng As New Random
    Function randomColour() As Color
        Return Color.FromArgb(255, rng.Next(255), rng.Next(255), rng.Next(255))
    End Function

    Const ImaginaryBorderSize As Integer = 16

    Function Top() As Rectangle
        Return New Rectangle(0, 0, Me.ClientSize.Width, ImaginaryBorderSize)
    End Function

    Function Left() As Rectangle
        Return New Rectangle(0, 0, ImaginaryBorderSize, Me.ClientSize.Height)
    End Function

    Function Bottom() As Rectangle
        Return New Rectangle(0, Me.ClientSize.Height - ImaginaryBorderSize, Me.ClientSize.Width, ImaginaryBorderSize)
    End Function

    Function Right() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - ImaginaryBorderSize, 0, ImaginaryBorderSize, Me.ClientSize.Height)
    End Function

    Function TopLeft() As Rectangle
        Return New Rectangle(0, 0, ImaginaryBorderSize, ImaginaryBorderSize)
    End Function

    Function TopRight() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - ImaginaryBorderSize, 0, ImaginaryBorderSize, ImaginaryBorderSize)
    End Function

    Function BottomLeft() As Rectangle
        Return New Rectangle(0, Me.ClientSize.Height - ImaginaryBorderSize, ImaginaryBorderSize, ImaginaryBorderSize)
    End Function

    Function BottomRight() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - ImaginaryBorderSize, Me.ClientSize.Height - ImaginaryBorderSize, ImaginaryBorderSize, ImaginaryBorderSize)
    End Function

    Public Const WM_NCLBUTTONDOWN As Integer = 161  'Detect if left mouse is pressed down
    Public Const HT_CAPTION As Integer = 2

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Me.Opacity = TrackBar1.Value / 100
        Label1.Text = TrackBar1.Value.ToString + "%"
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim result3 As DialogResult = MessageBox.Show("If you found this tool helpful" & vbCrLf & "please consider to Follow me or Like my Thread", "You are asked!", MessageBoxButtons.YesNo)

        ' Test DialogResult.
        If result3 = DialogResult.Yes Then
            Process.Start("https://forums.mihoyo.com/genshin/accountCenter/postList?id=87317176")
        End If


    End Sub

    <DllImport("User32")> Private Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer  'DLLImport for Moveable Window without Border
    End Function
    <DllImport("User32")> Private Shared Function ReleaseCapture() As Boolean   'DLLImport for Moveable Window without Border
    End Function

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown    'Be able to move the window with left mouse although it is borderless
        If (e.Button = MouseButtons.Left) Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        If (e.Button = MouseButtons.Left) Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        AddHandler PictureBox1.DragDrop, AddressOf PictureBox1_DragDrop
        AddHandler PictureBox1.DragEnter, AddressOf PictureBox1_DragEnter

    End Sub

    Private Sub PictureBox1_DragDrop(sender As Object, e As DragEventArgs) Handles PictureBox1.DragDrop

        Dim picbox As PictureBox = CType(sender, PictureBox)

        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

        If files.Length <> 0 Then
            Try
                picbox.BackgroundImage = Image.FromFile(files(0))
            Catch ex As Exception
                MessageBox.Show("Problem opening file ")
            End Try
        End If
    End Sub

    Private Sub PictureBox1_DragEnter(sender As Object, e As DragEventArgs) Handles PictureBox1.DragEnter

        Label2.Hide()

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

End Class
