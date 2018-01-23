Attribute VB_Name = "SetZonas"
Option Explicit

Function SeteoFactura(PrinterOCX As PrinterFiscal) As Boolean
' **************************************************************************
' Propiedad intelectual EPSON ARGENTINA S.A.
' Programador: Gomez Guillermo
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************

'Ejemplo de seteo de Zona, Headers y Trailers para una factura
'TAMAÑO  : Form.continuo 12'' x 10''
'IMPRESOR: LX-300 Fiscal, FX-880 Fiscal

'PrinterOCX : es el objeto PrinterFiscal (OCX) a travez del cual
'             se accede a la impresora fiscal

Dim Continuar As Boolean

'** DEFINICION GENERAL DEL FORMULARIO

    '** Configura el tamaño de la hoja, calculando las líneas para 1/6 de pulgada y
    '** las columnas en 12 caracteres por pulgada
    Continuar = PrinterOCX.SetPaperSize(90, 72)

    '** Esta instruccion configura la estacion para la emision Facturas A por Main Sheet
    '** no se debe eliminar
    If Continuar Then
        Continuar = PrinterOCX.SetPreference("P", "D", "M", "F", "A", "N")
    End If
    '** Esta instruccion configura la estacion para la emision de documento no fiscal
    '** no debe eliminarse, si es necesario se puede modificar
    If Continuar Then
        Continuar = PrinterOCX.SetPreference("P", "P", "R", "S", "U", "90", "72")
    End If
'** DEFINICION DE ZONAS
    '
    '** ZONA(010) Letra Comprobante
    If Continuar Then
        Continuar = PrinterOCX.SetZone(10, 43, 4, 44, 4)
    End If
    '** ZONA(022) Logo
    If Continuar Then
        Continuar = PrinterOCX.SetZone(22, 1, 4, 40, 4)
    End If
    '** ZONA(024) Fantasía y Razon social
    If Continuar Then
        Continuar = PrinterOCX.SetZone(24, 1, 5, 40, 7)
    End If
    '** ZONA(026) Datos Vendedor
    If Continuar Then
        Continuar = PrinterOCX.SetZone(26, 1, 9, 40, 14)
    End If
    '** ZONA(034) Nro Factura y fecha
    If Continuar Then
        Continuar = PrinterOCX.SetZone(34, 49, 5, 87, 7)
    End If
    '** ZONA(036) Cuit e impuestos
    If Continuar Then
        Continuar = PrinterOCX.SetZone(36, 45, 9, 87, 14)
    End If
    '** ZONA(042) Datos del comprador
    If Continuar Then
        Continuar = PrinterOCX.SetZone(42, 1, 16, 40, 20)
    End If
    '** ZONA(044) IVA Comprador
    If Continuar Then
        Continuar = PrinterOCX.SetZone(44, 1, 21, 40, 22)
    End If
    '** ZONA(052) CUIT Comprador
    If Continuar Then
        Continuar = PrinterOCX.SetZone(52, 45, 16, 87, 20)
    End If
    '** ZONA(054) Nro remitos
    If Continuar Then
        Continuar = PrinterOCX.SetZone(54, 45, 21, 87, 22)
    End If
    '** ZONA(060) Zona de detalle de venta
    If Continuar Then
        Continuar = PrinterOCX.SetZone(60, 1, 23, 87, 42)
    End If
    '** ZONA(061) Cantidad Item
    If Continuar Then
        Continuar = PrinterOCX.SetZone(61, 1, 23, 12, 42)
    End If
    '** ZONA(062) Descripción Item
    If Continuar Then
        Continuar = PrinterOCX.SetZone(62, 14, 23, 34, 42)
    End If
    '** ZONA(063) Precio Unitario Item
    If Continuar Then
        Continuar = PrinterOCX.SetZone(63, 36, 23, 48, 42)
    End If
    '** ZONA(064) Tasa de IVA Item
    If Continuar Then
        Continuar = PrinterOCX.SetZone(64, 50, 23, 59, 42)
    End If
    '** ZONA(065) Tasa Ajuste Variable Item
    If Continuar Then
        Continuar = PrinterOCX.SetZone(65, 61, 23, 70, 42)
    End If
    '** ZONA(066) Precio total Item
    If Continuar Then
        Continuar = PrinterOCX.SetZone(66, 72, 23, 87, 42)
    End If
    '** Esta instruccion configura el offset que se usará para imprimir los subtotales
    '** debe ser menor o igual a las filas de la zona 60, por omisión se toma
    '** las filas de la zona 60
    If Continuar Then
        Continuar = PrinterOCX.SetPreference("P", "C", "O", "901", "0", "20")
    End If
    '** ZONA(070) Mensajes de advertencia fiscal
    If Continuar Then
        Continuar = PrinterOCX.SetZone(70, 1, 23, 87, 42)
    End If
    '** ZONA(081) Descripción Percepcion de IVA RI
    If Continuar Then
        Continuar = PrinterOCX.SetZone(81, 33, 23, 41, 49)
    End If
    '** ZONA(082) Monto Percepcion de IVA RI
    If Continuar Then
        Continuar = PrinterOCX.SetZone(82, 43, 23, 54, 49)
    End If
    '** ZONA(083) Tasa de acrecentamiento RNI
    If Continuar Then
        Continuar = PrinterOCX.SetZone(83, 33, 23, 41, 49)
    End If
    '** ZONA(084) Monto de acrecentamiento RNI
    If Continuar Then
        Continuar = PrinterOCX.SetZone(84, 43, 23, 54, 49)
    End If
    '** ZONA(085) Descr.otras percep.
    If Continuar Then
        Continuar = PrinterOCX.SetZone(85, 56, 23, 62, 49)
    End If
    '** ZONA(086) Monto otras percep.
    If Continuar Then
        Continuar = PrinterOCX.SetZone(86, 64, 23, 75, 49)
    End If
    '** ZONA(090) Zona de subtotal y cierre
    If Continuar Then
        Continuar = PrinterOCX.SetZone(90, 1, 23, 87, 49)
    End If
    '** ZONA(091) Conceptos no gravados
    If Continuar Then
        Continuar = PrinterOCX.SetZone(91, 1, 23, 10, 49)
    End If
    '** ZONA(092) Tasa de IVA
    If Continuar Then
        Continuar = PrinterOCX.SetZone(92, 12, 23, 17, 49)
    End If
    '** ZONA(093) Monto de IVA
    If Continuar Then
        Continuar = PrinterOCX.SetZone(93, 19, 23, 31, 49)
    End If
    '** ZONA(094) Percepciones de IVA
    If Continuar Then
        Continuar = PrinterOCX.SetZone(94, 33, 23, 54, 49)
    End If
    '** ZONA(095) Otras percepciones
    If Continuar Then
        Continuar = PrinterOCX.SetZone(95, 56, 23, 75, 49)
    End If
    '** ZONA(097) Subtotal
    If Continuar Then
        Continuar = PrinterOCX.SetZone(97, 77, 23, 87, 49)
    End If
    '** ZONA(100) Zona de total documento
    If Continuar Then
        Continuar = PrinterOCX.SetZone(100, 1, 50, 87, 50)
    End If
    '** ZONA(101) Título de Total documento
    If Continuar Then
        Continuar = PrinterOCX.SetZone(101, 1, 50, 55, 50)
    End If
    '** ZONA(102) Monto de Total documento
    If Continuar Then
        Continuar = PrinterOCX.SetZone(102, 56, 50, 87, 50)
    End If
    '** ZONA(110) Zona de detalle de pagos
    If Continuar Then
        Continuar = PrinterOCX.SetZone(110, 1, 51, 87, 55)
    End If
    '** ZONA(111) Descripción de Pagos
    If Continuar Then
        Continuar = PrinterOCX.SetZone(111, 1, 51, 39, 55)
    End If
    '** ZONA(112) Monto de Pagos
    If Continuar Then
        Continuar = PrinterOCX.SetZone(112, 41, 51, 87, 55)
    End If
    '** ZONA(120) Leyendas de Pie de Factura
    If Continuar Then
        Continuar = PrinterOCX.SetZone(120, 1, 56, 87, 58)
    End If
    '** ZONA(142) Logo Fiscal
    If Continuar Then
        Continuar = PrinterOCX.SetZone(142, 1, 59, 40, 63)
    End If
    '** ZONA(144) Original / Copia
    If Continuar Then
        Continuar = PrinterOCX.SetZone(144, 45, 59, 87, 63)
    End If
'** DEFINICION DE HEADERS y TRAILERS
    '
    '** HEADER/TRAILER (01) Nombre de Fantasía - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 1, Chr(127))
    End If
    '** HEADER/TRAILER (02) Nombre de Fantasía - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 2, Chr(127))
    End If
    '** HEADER/TRAILER (03) Nombre de Fantasía - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 3, Chr(127))
    End If
    '** HEADER/TRAILER (04) Nombre de Fantasía - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 4, Chr(127))
    End If
    '** HEADER/TRAILER (05) Nombre de Fantasía - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 5, Chr(127))
    End If
    '** HEADER/TRAILER (11) Leyenda de pie 11
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 11, "Gracias por su compra")
    End If
    '** HEADER/TRAILER (12) Leyenda de pie 12 - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 12, Chr(127))
    End If
    '** HEADER/TRAILER (13) Leyenda de pie 13 - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 13, Chr(127))
    End If
    '** HEADER/TRAILER (14) Leyenda de pie 14 - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 14, Chr(127))
    End If
    '** HEADER/TRAILER (50) Domicilio Comercial
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 50, "Dato fijo")
    End If
    '** HEADER/TRAILER (51) Domicilio Comercial
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 51, "Capital Federal")
    End If
    '** HEADER/TRAILER (52) Domicilio Comercial - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 52, Chr(127))
    End If
    '** HEADER/TRAILER (53) Domicilio Fiscal
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 53, "Belgrano 951")
    End If
    '** HEADER/TRAILER (54) Domicilio Fiscal
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 54, "Capital Federal")
    End If
    '** HEADER/TRAILER (55) Domicilio Fiscal - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 55, Chr(127))
    End If
    '** HEADER/TRAILER (57) Impuestos Vendedor
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 57, "Ingresos Brutos")
    End If
    '** HEADER/TRAILER (58) Impuestos Vendedor - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 58, Chr(127))
    End If
    '** HEADER/TRAILER (59) Impuestos Vendedor - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 59, Chr(127))
    End If
    '** HEADER/TRAILER (60) Impuestos Vendedor - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 60, Chr(127))
    End If
    '** HEADER/TRAILER (61) Impuestos Vendedor - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 61, Chr(127))
    End If
    '** HEADER/TRAILER (62) Fecha de inicio de actividades
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 62, "Inicio de actividades 01/01/2000")
    End If
    '** HEADER/TRAILER (63) Leyendas Comerciales - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 63, Chr(127))
    End If
    '** HEADER/TRAILER (64) Leyendas Comerciales - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 64, Chr(127))
    End If
    '** HEADER/TRAILER (65) Leyendas Comerciales - Vacio
    If Continuar Then
        Continuar = PrinterOCX.SetGetHeaderTrailer("S", 65, Chr(127))
    End If
    
SeteoFactura = Continuar

' **************************************************************************
' Este software se entrega con fines didácticos y sin garantia alguna.
' EPSON NO ASUME responsabilidad legal alguna. El programador usa esta información
' bajo su propio riesgo y responsabilidad.
' **************************************************************************

End Function
