import { ChangeEvent, useState } from "react";

import axios from "axios";

function FileUploadSingle() {
  const [file, setFile] = useState();

  const handleFileChange = (e) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };

  const handleUploadClick = () => {
    if (!file) {
      return;
    }

    const headers = {
      "Content-Type": file.type,
      "Content-Length": `${file.size}`, // 👈 Headers need to be a string
      //"Access-Control-Allow-Origin": "*",
      };
      
      const formData = new FormData();
      formData.append("file", file);

    axios
      .post("https://localhost:7188/api/file/uploadhtml", formData, { headers })
      .then((res) => res.json())
      .then((data) => console.log(data))
      .catch((err) => console.error(err));

    // 👇 Uploading the file using the fetch API to the server
    // fetch("https://localhost:7188/api/File/Upload", {
    //   method: "POST",
    //   body: file,
    //   // 👇 Set headers manually for single file upload
    //   headers: {
    //     "content-type": file.type,
    //     "content-length": `${file.size}`, // 👈 Headers need to be a string
    //     "Access-Control-Allow-Origin": "*",
    //   },
    // })
    //   .then((res) => res.json())
    //   .then((data) => console.log(data))
    //   .catch((err) => console.error(err));
  };

  return (
    <div>
      <input type="file" onChange={handleFileChange} />

      <div>{file && `${file.name} - ${file.type}`}</div>

      <button onClick={handleUploadClick}>Upload</button>
    </div>
  );
}

export default FileUploadSingle;
