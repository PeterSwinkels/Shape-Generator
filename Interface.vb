'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Math
Imports System.Windows.Forms

'This class contains this program's main interface window's procedures.
Public Class InterfaceWindow
   Private Const DEGREES_PER_RADIAN As Double = 180 / PI   'Defines the number of degrees per radian.

   Private ColorDialogO As New ColorDialog With {.Color = Color.White}   'Contains the color dialog.
   Private ToolTipO As New ToolTip   'Defines this window's tooltip.

   'This procedure initializes this window.
   Public Sub New()
      Try
         Me.InitializeComponent()

         With My.Computer.Screen.WorkingArea
            Me.ClientSize = New Size(CInt(.Width / 1.5), CInt(.Height / 1.5))
         End With

         ToolTipO.SetToolTip(Me, "Click any where to generate a shape. Press: C to select a color - F1 to clear the window.")

         Me.Text = ProgramInformation()
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure returns a shape generated using the specified parameters.
   Private Function GenerateShape(Corners As Integer, x As Integer, y As Integer, Optional Angle As Integer = 0, Optional Radii() As Integer = Nothing) As PointF()
      Try
         Dim Radian As Integer = Radii.GetLowerBound(0)
         Dim xy As New List(Of PointF)

         For Degree As Integer = 0 To 360 Step CInt(360 / Corners)
            xy.Add(New PointF(CSng(Sin((Degree + Angle) / DEGREES_PER_RADIAN) * Radii(Radian)) + x, CSng(Cos((Degree + Angle) / DEGREES_PER_RADIAN) * Radii(Radian)) + y))
            If Radian = Radii.GetUpperBound(0) Then Radian = Radii.GetLowerBound(0) Else Radian += 1
         Next Degree

         Return xy.ToArray()
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return {}
   End Function

   'This procedure handles any errors the occur.
   Private Sub HandleError(ExceptionO As Exception)
      Try
         If MessageBox.Show(ExceptionO.Message, My.Application.Info.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = DialogResult.Cancel Then Application.Exit()
      Catch
         Application.Exit()
      End Try
   End Sub

   'This procedure handles the user's key strokes.
   Private Sub InterfaceWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
      Try
         Select Case e.KeyCode
            Case Keys.C
               ColorDialogO.ShowDialog()
            Case Keys.F1
               Me.CreateGraphics.Clear(Color.Black)
         End Select
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command to generate and draw a shape based on the parameters specified by the user.
   Private Sub InterfaceWindow_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
      Try
         Static Corners As String = "12"
         Static Angle As String = "0"
         Static Radii As String = "100;50"

         Angle = InputBox("Angle:",, Angle)
         If Not Angle = Nothing Then
            Corners = InputBox("Number of corners:",, Corners)
            If Not Corners = Nothing Then
               Radii = InputBox("Radii:",, Radii)
               If Not Radii = Nothing Then
                  Me.CreateGraphics.FillPolygon(New SolidBrush(ColorDialogO.Color), GenerateShape(Integer.Parse(Corners), e.X, e.Y, Integer.Parse(Angle), (From Radius In Radii.Split(";"c) Select Integer.Parse(Radius)).ToArray()))
               End If
            End If
         End If
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure returns this program's information.
   Private Function ProgramInformation() As String
      Try
         With My.Application.Info
            Return $"{ .Title} v{ .Version}, by: { .CompanyName}"
         End With
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function
End Class