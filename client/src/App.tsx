import * as React from "react";
import {ChangeEvent, FormEvent, useEffect, useState} from "react";
import {MessageInfo, MessageType} from "./types/Message";
import {CheckFileStatusApiResponse, FileConversionStatus, UploadFileApiResponse} from "./types/ApiResponses";
import {FileInfo} from "./types/FileInfo";
import axios, {AxiosError} from "axios";

import {Button, Container, Form, Header, Message, Table,} from "semantic-ui-react";

import {environment} from "./environments/environment";

import "semantic-ui-css/semantic.min.css";

const App = () => {
    return (
        <div className="App">
            <Header as="h1" textAlign="center" content="Загрузчик файлов"/>
            <FileUploader/>
        </div>
    );
};

const FileUploader = () => {
    const [file, setFile] = useState<File | null>(null);
    const [uploadedFiles, setUploadedFiles] = useState<FileInfo[]>([]);
    const [messageVisible, setMessageVisible] = useState(false);
    const [message, setMessage] = useState<MessageInfo>({
        type: MessageType.Negative
    });

    const showMessage = (type: MessageType, text: string) => {
        setMessage({
            type: type,
            text: text
        })
        setMessageVisible(true)
    }

    useEffect(() => {
        setTimeout(() => {
            if (messageVisible) {
                setMessageVisible(false);
            }
        }, 5000);
    }, [messageVisible]);

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if(!event.target.files) return;
        setFile(event.target.files[0]);
    };

    const handleFileRemove = (fileId: string) => {
        const updatedFiles = uploadedFiles.filter((file) => file.id !== fileId);
        setUploadedFiles(updatedFiles);
    };

    const handleDismissMessage = () => {
        setMessageVisible(false);
    };

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!file) {
            showMessage(MessageType.Negative, "Выберите файл для загрузки");
            return;
        }

        const formData = new FormData();
        formData.append("file", file);

        try {
            const response = await axios.post(
                `${environment.apiBaseUrl}/file`,
                formData,
                {
                    headers: {
                        "Content-Type": "multipart/form-data",
                    },
                }
            );

            showMessage(MessageType.Positive, "Файл успешно загружен!");

            setFile(null); // Сброс выбранного файла

            const fileId: UploadFileApiResponse = response.data;

            // Обновление списка загруженных файлов
            setUploadedFiles([
                {
                    id: fileId,
                    name: file.name,
                    uploadTime: new Date()
                },
                ...uploadedFiles,
            ]);
        } catch (error) {
            showMessage(MessageType.Negative, "Ошибка отправки файла.");
            console.error("Upload error:", error);
        }
    };

    const handleFileDownload = async (fileId: string) => {
        try {
            //Проверяем готов ли файл
            const checkFileStatusApiResponse = await checkIfFileIsReady(fileId);
            if (checkFileStatusApiResponse.conversionStatus !== FileConversionStatus.Success) {
                showMessage(MessageType.Negative, "Файл еще не обработан сервером, подождите.");
                return;
            }

            // Делаем GET запрос для скачивания файла по fileId
            const response = await axios.get(
                `${environment.apiBaseUrl}/file/${fileId}`,
                {
                    responseType: "blob", // Устанавливаем тип ответа как blob
                }
            );

            // Создаем ссылку на скачивание файла
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement("a");
            link.href = url;
            link.setAttribute("download", `file-${fileId}.pdf`); // Название файла для скачивания
            document.body.appendChild(link);
            link.click();

            // Очищаем ссылку после скачивания
            link.parentNode?.removeChild(link);
        } catch (error) {
            const err = error as AxiosError
            showMessage(MessageType.Negative, `Ошибка при скачивании файла. ${error} ${err.response?.data}`);
            console.error("Download error:", error);
        }
    };

    const checkIfFileIsReady = async (fileId : string) => {
        try {
            const response = await axios.get(
                `${environment.apiBaseUrl}/file/${fileId}/status`,
                {
                    validateStatus: () => true,
                }
            );

            return response.data as CheckFileStatusApiResponse;
        } catch (error) {
            const err = error as AxiosError
            showMessage(MessageType.Negative, `Ошибка при проверке статуса файла файла. ${error} ${err.response?.data}`);
            console.error("Check file status error:", error);
            throw error;
        }
    };

    return (
        <Container>
            <Form onSubmit={handleSubmit}>
                <Form.Input type="file" onChange={handleFileChange}/>
                <Form.Button
                    type="submit"
                    content="Загрузить"
                    fluid
                    basic
                    color="teal"
                />
            </Form>
            <Message
                positive={message.type === MessageType.Positive}
                negative={message.type === MessageType.Negative}
                content={message.text}
                onDismiss={handleDismissMessage}
                hidden={!messageVisible}
            />
            <Table basic="very">
                <Table.Body>
                    {uploadedFiles.map((file, index) => (
                        <Table.Row key={index}>
                            <Table.Cell>{file.name}</Table.Cell>
                            <Table.Cell>{file.uploadTime.toUTCString()}</Table.Cell>
                            <Table.Cell>
                                <Button
                                    negative
                                    onClick={() => handleFileRemove(file.id)}
                                    content="Удалить"
                                    floated="right"
                                />
                                <Button
                                    positive
                                    onClick={() => handleFileDownload(file.id)}
                                    content="Скачать"
                                    floated="right"
                                />
                            </Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
            </Table>
        </Container>
    );
};

export default App;
