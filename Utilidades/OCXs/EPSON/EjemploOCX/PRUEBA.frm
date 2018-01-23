VERSION 5.00
Object = "{B52C1CDE-38E9-11D5-98EC-00C0F01C6C81}#1.0#0"; "IFEPSON.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   5148
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   6012
   LinkTopic       =   "Form1"
   ScaleHeight     =   5148
   ScaleWidth      =   6012
   StartUpPosition =   3  'Windows Default
   Begin EPSON_Impresora_Fiscal.PrinterFiscal PrinterFiscal1 
      Left            =   4920
      Top             =   360
      _ExtentX        =   847
      _ExtentY        =   847
   End
   Begin VB.CommandButton Command1 
      Caption         =   "FACTURA ""A"" con Transporte"
      Height          =   375
      Index           =   12
      Left            =   120
      TabIndex        =   13
      Top             =   1080
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "SETEA diseño de FACTURA"
      Height          =   375
      Index           =   11
      Left            =   120
      TabIndex        =   12
      Top             =   4080
      Width           =   3015
   End
   Begin VB.CommandButton Command2 
      Caption         =   "CIERRE X"
      Height          =   495
      Index           =   0
      Left            =   3480
      TabIndex        =   8
      Top             =   960
      Width           =   1095
   End
   Begin VB.CommandButton Command2 
      Caption         =   "CIERRE Z"
      Height          =   495
      Index           =   1
      Left            =   3480
      TabIndex        =   9
      Top             =   480
      Width           =   1095
   End
   Begin VB.CommandButton Command1 
      Caption         =   "FACTURA ""A"""
      Height          =   375
      Index           =   9
      Left            =   120
      TabIndex        =   11
      Top             =   720
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "TIQUE FACTURA ""A"""
      Height          =   375
      Index           =   1
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "TIQUE"
      Height          =   375
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "ESTADO"
      Height          =   495
      Index           =   7
      Left            =   3480
      TabIndex        =   7
      Top             =   0
      Width           =   1095
   End
   Begin VB.CommandButton Command1 
      Caption         =   "DNF por SLIP"
      Height          =   375
      Index           =   8
      Left            =   120
      TabIndex        =   10
      Top             =   3600
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "HEADERS / TRAILERS"
      Height          =   375
      Index           =   6
      Left            =   120
      TabIndex        =   6
      Top             =   3240
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "CAJON"
      Height          =   375
      Index           =   5
      Left            =   120
      TabIndex        =   5
      Top             =   2880
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "HORA Y FECHA"
      Height          =   375
      Index           =   4
      Left            =   120
      TabIndex        =   4
      Top             =   2520
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "DNFH TC y OS"
      Height          =   375
      Index           =   3
      Left            =   120
      TabIndex        =   3
      Top             =   2040
      Width           =   3015
   End
   Begin VB.CommandButton Command1 
      Caption         =   "DNF"
      Height          =   375
      Index           =   2
      Left            =   120
      TabIndex        =   2
      Top             =   1680
      Width           =   3015
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' **************************************************************************
' Propiedad intelectual EPSON ARGENTINA S.A.
' Programador: Gomez Guillermo
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************
Dim Funciones() As String
Dim AyudaFunciones() As String
Dim Parametros() As String


Private Sub Command1_Click(Index As Integer)
' **************************************************************************
' Propiedad intelectual EPSON ARGENTINA S.A.
' Programador: Gomez Guillermo
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************
Dim respuesta As Boolean

Screen.MousePointer = 11

    Select Case Index
    
        Case 0
            'Tique
            respuesta = Me.PrinterFiscal1.OpenTicket("G")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendTicketItem("ARTICULO 1", "1000", "100", "2100", "M", "0", "0")
            If respuesta Then respuesta = Me.PrinterFiscal1.GetTicketSubtotal("P", "LINDO SUB")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendTicketPayment("PAGO1", "200", "T")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseTicket
            
        
        Case 1
            'Tique Factura
            respuesta = Me.PrinterFiscal1.OpenInvoice("T", "C", "A", "1", "P", "12", "I", "I", "PEPE", "LE BOU", "CUIT", "30614104712", "N", "LA", "PAMPA", "98", "REM 1", "REM 2", "G")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoiceItem("ARTICULO 1", "1000", "100", "2100", "M", "0", "0", "EXTRA", "EXTRA", "EXTRA", "1050", "0")
            If respuesta Then respuesta = Me.PrinterFiscal1.GetInvoiceSubtotal("P", "LINDO SUB")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoicePayment("PAGO1", "200", "T")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseInvoice("T", "A", "HOLA")
        
        Case 2
            'dnf
            respuesta = Me.PrinterFiscal1.OpenNoFiscal
            If respuesta Then respuesta = Me.PrinterFiscal1.SendNoFiscalText("á12345678à")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseNoFiscal
            
        Case 3
            'DNFH TC Y OS
            respuesta = Me.PrinterFiscal1.DNFHCreditCard("VISA", "12", "PEPE", "991231", "23", "46", "57", "89", "CONTA", "100", "2", "PESOS", "2", "4", "5", "7", "8", "44", "P", "P", "P")
            If respuesta Then respuesta = Me.PrinterFiscal1.DNFHDrugstore("MEDICUS", "CO 1", "CO 2", "CO 3", "123", "PEP", "991030", "ADRESS", "ADDRESS 2", "NADA", "12", "EXTRA 1", "EXTRA 2", "P", "P", "P", "P", "P")
            
        Case 4
            'Hora y Fecha
            respuesta = Me.PrinterFiscal1.SetGetDateTime("S", Format(Date, "YYMMDD"), Format(Time, "HHMMSS"))
            
        Case 5
            'cajon
            respuesta = Me.PrinterFiscal1.OpenCashDrawer("1")
            If respuesta Then respuesta = Me.PrinterFiscal1.OpenCashDrawer("2")
            
        Case 6
            'header /trailer
            respuesta = Me.PrinterFiscal1.SetGetHeaderTrailer("S", "1", "PRUEBA HEADER")
            If respuesta Then respuesta = Me.PrinterFiscal1.SetGetHeaderTrailer("G", "1")
            If respuesta Then MsgBox "El header obtenido es : " & Me.PrinterFiscal1.AnswerField_4
            
        Case 7
            respuesta = Me.PrinterFiscal1.Status
        
        Case 8
            'dnf por Slip
            respuesta = Me.PrinterFiscal1.SelectSlip
            If respuesta Then respuesta = Me.PrinterFiscal1.SetPaperSize(50, 88)
            If respuesta Then MsgBox "Inserte una hoja en la entrada de Slip", vbInformation, "ATENCION"
            If respuesta Then respuesta = Me.PrinterFiscal1.PrepareSlip
            If respuesta Then MsgBox "Se imprimirá un documento no fiscal por slip", vbInformation, "ATENCION"
            If respuesta Then respuesta = Me.PrinterFiscal1.OpenSlipNoFiscal
            If respuesta Then respuesta = Me.PrinterFiscal1.SendNoFiscalText("       E J E M P L O")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendNoFiscalText("S L I P   N O   F I S C A L")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendNoFiscalText(" P O R   O C X F I S C A L")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseNoFiscal
        
        Case 9
            'Factura
            respuesta = Me.PrinterFiscal1.OpenInvoice("F", "C", "A", "1", "P", "12", "I", "I", "PEPE", "LE BOU", "CUIT", "30614104712", "N", "LA", "PAMPA", "98", "REM 1", "REM 2", "C")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoiceItem("ARTICULO 1", "1000", "100", "2100", "M", "0", "0", "EXTRA", "EXTRA", "EXTRA", "1050", "0")
            'If respuesta Then respuesta = Me.PrinterFiscal1.GetInvoiceSubtotal("P", "LINDO SUB")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoicePayment("PAGO1", "200", "T")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseInvoice("F", "A", "HOLA")
        Case 10
            respuesta = Me.PrinterFiscal1.OpenInvoice("F", "C", "A", "1", "P", "12", "I", "I", "PEPE", "LE BOU", "CUIT", "30614104712", "N", "LA", "PAMPA", "98", "REM 1", "REM 2", "C")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoiceItem("ARTICULO 1", "1000", "100", "2100", "M", "0", "0", "EXTRA", "EXTRA", "EXTRA", "1050", "0")
            'If respuesta Then respuesta = Me.PrinterFiscal1.GetInvoiceSubtotal("P", "LINDO SUB")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoicePayment("PAGO1", "200", "T")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseInvoice("F", "A", "HOLA")
        Case 11 'setea diseño de factura
            respuesta = SeteoFactura(Me.PrinterFiscal1)
        Case 12 'Factura con transporte
            respuesta = Me.PrinterFiscal1.OpenInvoice("F", "C", "A", "1", "P", "12", "I", "I", "PEPE", "LE BOU", "CUIT", "30614104712", "N", "LA", "PAMPA", "98", "REM 1", "REM 2", "C")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoiceItem("ARTICULO 1", "1000", "100", "2100", "M", "0", "0", "EXTRA", "EXTRA", "EXTRA", "1050", "0")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoiceItem("ARTICULO 2", "1000", "1200", "2100", "M", "0", "0", "", "", "", "1050", "0")
            If respuesta Then respuesta = Me.PrinterFiscal1.TransportClose
            If respuesta Then MsgBox "Coloque la siguiente hoja"
            If respuesta Then respuesta = Me.PrinterFiscal1.TransportOpen
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoiceItem("ARTICULO 3", "1000", "1000", "2100", "M", "0", "0", "", "", "", "1050", "0")
            'If respuesta Then respuesta = Me.PrinterFiscal1.GetInvoiceSubtotal("P", "LINDO SUB")
            If respuesta Then respuesta = Me.PrinterFiscal1.SendInvoicePayment("PAGO1", "3000", "T")
            If respuesta Then respuesta = Me.PrinterFiscal1.CloseInvoice("F", "A", "HOLA")

    End Select

Screen.MousePointer = 1

    MsgBox respuesta & Chr$(13) & Me.PrinterFiscal1.FiscalStatus & Chr$(13) & Me.PrinterFiscal1.PrinterStatus

End Sub

Private Sub Command2_Click(Index As Integer)
' **************************************************************************
' Propiedad intelectual EPSON ARGENTINA S.A.
' Programador: Gomez Guillermo
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************

Select Case Index

    Case 0
        respuesta = Me.PrinterFiscal1.CloseJournal("X", "P")
        
    Case 1
        respuesta = Me.PrinterFiscal1.CloseJournal("Z")
        
       
End Select
    MsgBox respuesta & Chr$(13) & Me.PrinterFiscal1.FiscalStatus & Chr$(13) & Me.PrinterFiscal1.PrinterStatus
    
End Sub


Private Sub Form_Load()
' **************************************************************************
' Propiedad intelectual EPSON ARGENTINA S.A.
' Programador: Gomez Guillermo
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************
Dim i As Integer
Dim Nombre As String
Dim Ayuda As String

ReDim Funciones(1)
ReDim AyudaFunciones(1)

End Sub
Function AddParam(NewParam As String, Optional Reset = False) As Integer
' **************************************************************************
' Propiedad intelectual EPSON ARGENTINA S.A.
' Programador: Gomez Guillermo
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************

'Agrega un parametro a la lista
Dim i As Integer
If Reset Then
    i = 0
    ReDim Parametros(i)
    Parametros(i) = NewParam
Else
    i = UBound(Parametros) + 1
    
    ReDim Preserve Parametros(i)
    Parametros(i) = NewParam
End If



End Function

