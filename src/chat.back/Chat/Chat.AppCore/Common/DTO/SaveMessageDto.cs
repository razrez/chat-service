namespace Chat.AppCore.Common.DTO;

public record SaveMessageDto(string Room, string User, string Message);