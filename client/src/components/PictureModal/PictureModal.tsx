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
}

const PictureModal: React.FC<PictureModalProps> = ({
  isOpen,
  onClose,
  picture,
  onPrev,
  onNext,
}) => {
  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <img
          src={picture?.imageUrl}
          alt={picture?.alias}
          className={styles.modalImage}
        />
        <div className={styles.modalTextBox}>
          <h4
            className={styles.title}
          >{`${picture?.name} (${picture?.year})`}</h4>
          <p
            className={styles.modalText}
          >{`${picture?.cinema.name}, ${picture?.cinema.city}`}</p>
        </div>
      </div>
      <button
        className={`${styles.arrowButton} ${styles.left}`}
        onClick={onPrev}
      >
        &lt;
      </button>
      <button
        className={`${styles.arrowButton} ${styles.right}`}
        onClick={onNext}
      >
        &gt;
      </button>
      {picture?.zinger ? (
        <div className={styles.zinger}>{`"${picture?.zinger}"`}</div>
      ) : null}
      ;
    </>,
    document.getElementById("portal")!
  );
};

export default PictureModal;
