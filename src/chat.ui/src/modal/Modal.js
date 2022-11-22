import React from 'react'
import './Modal.css'

const Modal = props => {
    if (!props.show) {
        return null
    }
    return (
        <div className="Modal">
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">Введите метаданные файла</h5>
                </div>
                <div className="modal-body">
                    <p>Здесь могла быть ваша реклама</p>
                </div>
                <div className="modal-footer">
                    <button onClick={props.onClose} className="button">Send metadata</button>
                </div>
            </div>
        </div>
    )
}

function MetaDataForTxt(props) {
    return <h1>Залили .txt файл бля буду</h1>
}



export default Modal