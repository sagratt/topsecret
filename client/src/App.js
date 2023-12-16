import React from 'react';
import FileUploader from './components/file/file-uploader.component'; // Обновленный путь импорта до компонента FileUploader
import Header from './components/header/header.component';
import './App.css'

const App = () => {
  return (
    <div className='App'>
      <Header />
      <FileUploader />
    </div>
  );
};

export default App;