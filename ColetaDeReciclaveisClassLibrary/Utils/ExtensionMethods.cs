namespace ColetaDeReciclaveisClassLibrary.Utils
{
    public static class CallbackStatusExtensionMethods
    {
        public static string CallbackStatusToText(this StaticsInfo.CALLBACK_STATUS callbackStatus)
        {
            switch (callbackStatus)
            {
                case StaticsInfo.CALLBACK_STATUS.ERROR:
                    return "Ocorreu Um Erro";
                case StaticsInfo.CALLBACK_STATUS.FINISHED:
                    return "Finalizado";
                case StaticsInfo.CALLBACK_STATUS.FINISHED_WITH_ERROR:
                    return "Finalizado Com Erro";
                case StaticsInfo.CALLBACK_STATUS.IN_PROGRESS:
                    return "Em Progresso";
                case StaticsInfo.CALLBACK_STATUS.STARTED:
                    return "Iniciando";
                case StaticsInfo.CALLBACK_STATUS.UNABLE_TO_ACCESS:
                    return "Não Foi Possível Acessar";
                default:
                    return "Resultado Desconhecido";
            }
        }
	}
}
