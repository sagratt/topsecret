type UploadFileApiResponse = string

type CheckFileStatusApiResponse = {
    fileId: string,
    conversionStatus: FileConversionStatus
}

enum FileConversionStatus {
    ReadyForConversion,
    InProgress,
    Success,
    Failure = 10
}

export {
    type UploadFileApiResponse,
    type CheckFileStatusApiResponse,
    FileConversionStatus
};