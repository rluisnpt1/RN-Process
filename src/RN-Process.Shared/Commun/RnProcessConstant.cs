using System;

namespace RN_Process.Shared.Commun
{
    public static class RnProcessConstant
    {
        public const string Open = "Open";
        public const string Complete = "Complete";

        public const string ColumnsBaseIntrum = "intrum_Column1";
        public const string ColumnsBaseClient = "client_Column1";
        public const string MsgCanNotSaveDuplicateTask = "Cannot save duplicate Tasks.";


        public static readonly string BaseTestWorkFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string BaseWorkFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);


        public static readonly string[] AvailableColumnsIntrum =
        {
            "NIF_TITULAR", "NIF_GESTOR", "TIPO_DEUDA_ID", "CODIGO_CLIENTE", "CODIGO_SUC", "CODIGO_CLIENTE_PAGO",
            "CODIGO_SUC_PAGO", "CLAVE_DEUDA", "ESTADO", "F_GESTION", "PERIODO_GESTION", "N_CUENTA", "NOMINAL", "SALDO",
            "PERIODICIDAD", "F_PRIM_VENCIM", "N_CUOTA_IMPAGADA", "F_ULT_VENCIM", "N_CUOTAS", "ENVIADO", "TOTAL_DEUDA",
            "OBJETO_FINANCIADO", "OBSERVACIONES", "F_ASIGNACION", "CODIGO_OFICINA", "COD_OFICINA", "CODIGO_ZONA",
            "CODIGO_ZONA_PAGO", "CODIGO_DGC", "CODIGO_DGC_PAGO", "NUM_RELACIONADO", "F_DEVOLUCION", "ESTADO_FALLIDO",
            "TIPO_MONEDA_ORIGEN", "CAPITAL", "INTERESES", "GASTOS", "P_INTERES", "F_ULT_LIQUI", "CLIENTE_PAGO_ID",
            "AGENTE_ID", "RESULTADO_GESTION", "N_EXPEDIENTE", "GASTOS_DEUDOR", "GASTOS_LEGAL", "LEGAL_CASE_ID",
            "OUTLAYS", "IJ_FEES", "INTERES_IJ", "INTERES_CLIENTE", "INTERES_CTE_INI", "F_REGISTRATION_INTEREST",
            "IS_NOT_IN_ANW", "DATE_INCLUDED_ANW", "ORIGIN_ID"
        };
    }
}