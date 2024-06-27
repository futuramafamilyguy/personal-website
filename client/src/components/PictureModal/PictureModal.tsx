import React from "react";
import ReactDom from "react-dom";

import { Picture } from "../../types/Picture";
import styles from "./PictureModal.module.css";

interface PictureModalProps {
  isOpen: boolean;
  onClose: () => void;
  picture: Picture | null;
  onPrev: () => void;
  onNext: () => void;
  prev: boolean;
  next: boolean;
}

const PictureModal: React.FC<PictureModalProps> = ({
  isOpen,
  onClose,
  picture,
  onPrev,
  onNext,
  prev,
  next,
}) => {
  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.imageContainer}>
          <img
            src={picture?.imageUrl}
            alt={picture?.alias}
            className={styles.modalImage}
          />
        </div>
        <div className={styles.modalTextBox}>
          <h4
            className={styles.title}
          >{`${picture?.name} (${picture?.yearReleased})`}</h4>
          <p
            className={styles.modalText}
          >{`${picture?.cinema.name}, ${picture?.cinema.city}`}</p>
        </div>
      </div>
      {prev ? (
        <button
          className={`${styles.arrowButton} ${styles.left}`}
          onClick={onPrev}
        >
          &lt;
        </button>
      ) : null}
      {next ? (
        <button
          className={`${styles.arrowButton} ${styles.right}`}
          onClick={onNext}
        >
          &gt;
        </button>
      ) : null}
      {picture?.zinger ? (
        <div className={styles.zinger}>{`"${picture?.zinger}"`}</div>
      ) : null}
      ;
    </>,
    document.getElementById("portal")!
  );
};

export default PictureModal;
