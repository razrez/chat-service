import React, { useState } from "react";
import "./FileUploader.css";

export const FileUploader = () => {
    const [image, setImage] = useState();
    const [imageURL, setImageURL] = useState();
    const fileReader = new FileReader();
    fileReader.onloadend = () => {
        setImageURL(fileReader.result);
    };
    const handleOnChange = (event) => {
        event.preventDefault();
        if (event.target.files && event.target.files.length) {
            const file = event.target.files[0];
            setImage(file);
            fileReader.readAsDataURL(file);
        }
    };

    const handleDrop = (event) => {
        event.preventDefault();
        event.stopPropagation();
        if (event.dataTransfer.files && event.dataTransfer.files.length) {
            setImage(event.dataTransfer.files[0]);
            fileReader.readAsDataURL(event.dataTransfer.files[0]);
        }
    };

    const handleDragEmpty = (event) => {
        event.preventDefault();
        event.stopPropagation();
    };

    return (
        <form className="file-uploader">
            <label
                htmlFor="file-loader-button"
                className="file-uploader__custom-button"
            >
                Upload file
            </label>
            <label
                htmlFor="file-loader-button"
                className="file-uploader__send-button"
            >
                Send file
            </label>
            <input
                id="file-loader-button"
                type="file"
                className="file-uploader__upload-button"
                onChange={handleOnChange}
            />
            <img
                src={imageURL ? imageURL : "no_photo.jpg"}
                className="file-uploader__preview"
                alt="preview"
                onDrop={handleDrop}
                onDragEnter={handleDragEmpty}
                onDragOver={handleDragEmpty}
            />
            <div className="file-uploader__file-name">{image ? image.name : ""}</div>
        </form>
    );
};