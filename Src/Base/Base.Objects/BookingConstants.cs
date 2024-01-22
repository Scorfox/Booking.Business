using Base.Objects.Helpers;

namespace Base.Objects;

public sealed class BookingConstants
{
    public static string ConfigFile = "booking.conf";
    
    //TODO: Move to config file

    private static readonly string RmqEncryptedPass = "3gE3isQQppgP9xyqoyoHgA==";
    public static string RabbitMqUser = "admin";
    public static string RabbitMqPass = Cryptography.Decrypt(RmqEncryptedPass);
    public static string RabbitMqHost = "158.160.118.121";
    public static string RabbitMqPort = "5672";

    //END_TODO 
}

