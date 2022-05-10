public static class PersonTypeEnumExtension
{
    public static string ToEnumName(this PersonTypeEnum personType)
    {
        switch (personType)
        {
            case PersonTypeEnum.Empty:
                return "Selecciona una opción";
            case PersonTypeEnum.Passenger:
                return "Pasajero";
            case PersonTypeEnum.CrewMember:
                return "Miembro de la tripulación";
            case PersonTypeEnum.User:
                return "Usuario";
            default:
                return "";
        }
    }

    public static string ToEnumName(this PassengerTypeEnum passengerType)
    {
        switch (passengerType)
        {
            case PassengerTypeEnum.Empty:
                return "Selecciona una opción";
            case PassengerTypeEnum.Adult:
                return "Adulto";
            case PassengerTypeEnum.Baby:
                return "Bebé";
            case PassengerTypeEnum.Children:
                return "Niño";
            default:
                return "";
        }
    }
}
