namespace GestaoEventos.DTO
{
    public class AddNotificationDto
    {
        // Type: "email" ou "phone"
        public string Type { get; set; }
        public string Value { get; set; }
    }


}
