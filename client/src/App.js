import React, { useEffect, useState } from "react";
import axios from "axios";

import {
  Container,
  Header,
  Form,
  Button,
  Table,
  Message,
  Icon,
  Embed,
} from "semantic-ui-react";

import { environment } from "./environments/environment";

import "semantic-ui-css/semantic.min.css";

const App = () => {
  return (
    <div className="App">
      <Header as="h1" textAlign="center" content="Загрузчик файлов" />
      <FileUploader />
    </div>
  );
};

const FileUploader = () => {
  const [file, setFile] = useState(null);
  const [uploadedFiles, setUploadedFiles] = useState([]);
  const [messageVisible, setMessageVisible] = useState(false);
  const [message, setMessage] = useState({});

  useEffect(() => {
    setTimeout(() => {
      if (messageVisible) {
        setMessageVisible(false);
      }
    }, 5000);
  }, [messageVisible]);

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleFileRemove = (fileId) => {
    const updatedFiles = uploadedFiles.filter((file) => file.id !== fileId);
    setUploadedFiles(updatedFiles);
  };

  const handleDismissMessage = () => {
    setMessageVisible(false);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!file) {
      setMessage({
        positive: false,
        text: "Выберите файл для загрузки",
      });
      setMessageVisible(true);
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

      setMessage({ positive: true, text: "Файл успешно загружен!" });
      setMessageVisible(true);

      setFile(null); // Сброс выбранного файла

      var fileId = response.data;

      // Обновление списка загруженных файлов
      setUploadedFiles([
        {
          id: fileId,
          originalName: file.name,
          uploadTime: new Date().toUTCString(),
        },
        ...uploadedFiles,
      ]);
    } catch (error) {
      setMessageVisible(true);
      setMessage({ positive: false, text: "Ошибка отправки файла." });
      console.error("Upload error:", error);
    }
  };

  const handleFileDownload = async (fileId) => {
    try {
      //Проверяем готов ли файл
      var fileIsReadyResponseModel = await checkIfFileIsReady(fileId);
      if (fileIsReadyResponseModel.conversionStatus !== 2) {
        setMessage({
          positive: false,
          text: "Файл еще не обработан сервером, подождите.",
        });
        setMessageVisible(true);
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
      link.parentNode.removeChild(link);
    } catch (error) {
      setMessage({
        positive: false,
        text: `Ошибка при скачивании файла. ${error} ${await error.response.data.text()}`,
      });
      setMessageVisible(true);
      console.error("Download error:", error);
    }
  };

  const checkIfFileIsReady = async (fileId) => {
    try {
      const response = await axios.get(
        `${environment.apiBaseUrl}/file/${fileId}/status`,
        {
          validateStatus: () => true,
        }
      );
      return response.data;
    } catch (error) {
      setMessage({
        positive: false,
        text: `Ошибка при проверке статуса файла файла. ${error} ${await error.response.data.text()}`,
      });
      setMessageVisible(true);

      console.error("Check file status error:", error);
    }
  };

  return (
    <Container>
      <Form onSubmit={handleSubmit}>
        <Form.Input type="file" onChange={handleFileChange} />
        <Form.Button
          type="submit"
          content="Загрузить"
          fluid
          basic
          color="teal"
        />
      </Form>
      <Message
        positive={message.positive}
        content={message.text}
        onDismiss={handleDismissMessage}
        hidden={!messageVisible}
      />
      <Table basic="very">
        <Table.Body>
          {uploadedFiles.map((file, index) => (
            <Table.Row key={index}>
              <Table.Cell>{file.originalName}</Table.Cell>
              <Table.Cell>{file.uploadTime}</Table.Cell>
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
