using System;

namespace BleRedux
{
    public static class BluetoothConstants
    {
        public const string DeviceInformationService = "0000180a-0000-1000-8000-00805f9b34fb"; //0x180A

        //https://chromium.googlesource.com/chromium/src/+/ed572c2b742c58dfcb172b0e5b339a7119654440/device/fido/fido_ble_uuids.cc
        public const string kFidoServiceUUID = "0000fffd-0000-1000-8000-00805f9b34fb";
        public const string kFidoControlPointUUID = "f1d0fff1-deaa-ecee-b42f-c9ba7ed623bb";
        public const string kFidoStatusUUID = "f1d0fff2-deaa-ecee-b42f-c9ba7ed623bb";
        public const string kFidoControlPointLengthUUID = "f1d0fff3-deaa-ecee-b42f-c9ba7ed623bb";
        public const string kFidoServiceRevisionUUID = "00002a28-0000-1000-8000-00805f9b34fb";
        public const string kFidoServiceRevisionBitfieldUUID = "f1d0fff4-deaa-ecee-b42f-c9ba7ed623bb";

        //https://gist.github.com/MZachmann/36988eb989ee26b2afc850f281ab6c05
        //https://stackoverflow.com/questions/36212020/how-can-i-convert-a-bluetooth-16-bit-service-uuid-into-a-128-bit-uuid
        //https://github.com/intel/zephyr/blob/c1875360e52754a2b4963643ad08fe1b774af638/include/bluetooth/uuid.h
        public const string BT_UUID_DIS_MANUFACTURER_NAME = "00002a29-0000-1000-8000-00805f9b34fb"; //0x2a29
        public const string BT_UUID_DIS_MODEL_NUMBER_VAL = "00002a24-0000-1000-8000-00805f9b34fb"; //0x2a24
        public const string BT_UUID_DIS_FIRMWARE_REVISION = "00002a26-0000-1000-8000-00805f9b34fb"; //0x2a26
    }
}
