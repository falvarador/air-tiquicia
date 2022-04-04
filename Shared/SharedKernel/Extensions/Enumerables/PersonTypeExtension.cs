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
}
