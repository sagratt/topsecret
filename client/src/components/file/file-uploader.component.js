import React, { useState } from "react";
import axios from "axios";

import { environment } from "../../environments/environment";

import "./file-uploader.styles.css";

const FileUploader = () => {
  const [file, setFile] = useState(null);
  const [uploadStatus, setUploadStatus] = useState("");
  const [uploadedFiles, setUploadedFiles] = useState([]);

  const apiUrl = environment.apiBaseUrl;

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleFileRemove = (fileId) => {
    const updatedFiles = uploadedFiles.filter((file) => file.fileId !== fileId);
    setUploadedFiles(updatedFiles);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!file) {
      setUploadStatus("Выберите файл для загрузки");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await axios.post(
        `${apiUrl}/file/uploadhtml`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      setUploadStatus("Файл успешно загружен!");
      setFile(null); // Сброс выбранного файла

      // Обновление списка загруженных файлов
      setUploadedFiles([
        { fileId: response.data.fileId, originalName: file.name },
        ...uploadedFiles,
      ]);
    } catch (error) {
      setUploadStatus("Ошибка загрузки файла");
      console.error("Upload error:", error);
    }
  };

  const handleFileDownload = async (fileId) => {
    try {
      // Делаем GET запрос для скачивания файла по fileId
      const response = await axios.get(`${apiUrl}/file/downloadpdf?fileId=${fileId}`, {
        responseType: 'blob', // Устанавливаем тип ответа как blob
      });

      // Создаем ссылку на скачивание файла
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `file-${fileId}.pdf`); // Название файла для скачивания
      document.body.appendChild(link);
      link.click();

      // Очищаем ссылку после скачивания
      link.parentNode.removeChild(link);
    } catch (error) {
      console.error('Download error:', error);
    }
  };

  return (
    <div className="container">
      <form onSubmit={handleSubmit}>
        <input type="file" onChange={handleFileChange} className="file-input" />
        <button type="submit">Загрузить</button>
      </form>
      <p>{uploadStatus}</p>
      {uploadedFiles.length ? <h3>Файлы:</h3> : null}
      <ul className="file-list">
        {uploadedFiles.map((uploadedFile, index) => (
          <li key={index} className="file-item">
            {uploadedFile.originalName || uploadedFile.name}
            <button onClick={() => handleFileDownload(uploadedFile.fileId)} className="download">
              Скачать
            </button>
            <button onClick={() => handleFileRemove(uploadedFile.fileId)} className="remove">
              Удалить
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default FileUploader;
