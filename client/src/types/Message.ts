type MessageInfo = {
    type: MessageType,
    text?: string
}

enum MessageType {
    Positive,
    Negative
}

export {
    type MessageInfo,
    MessageType
}