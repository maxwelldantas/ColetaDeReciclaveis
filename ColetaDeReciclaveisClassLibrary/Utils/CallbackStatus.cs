using System;

namespace ColetaDeReciclaveisClassLibrary.Utils {
    public class CallbackStatus {

        public StaticsInfo.CALLBACK_STATUS CurrentStatus;

        public string CallbackMessage;

        public CallbackStatus(StaticsInfo.CALLBACK_STATUS currentStatus, string callbackMessage) {
            CurrentStatus = currentStatus;
            CallbackMessage = callbackMessage ?? throw new ArgumentNullException(nameof(callbackMessage));
        }
        public CallbackStatus() {
        }

    }
}
