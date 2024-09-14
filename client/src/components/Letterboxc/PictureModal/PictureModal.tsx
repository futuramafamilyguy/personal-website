import React from "react";
import ReactDom from "react-dom";

import Picture from "../../../types/Picture";
import styles from "./PictureModal.module.css";

import heartIcon from "../../../assets/svg/heart_filled.png";

interface PictureModalProps {
  isOpen: boolean;
  onClose: () => void;
  picture: Picture | null;
  handlePrev: () => void;
  handleNext: () => void;
  prev: boolean;
  next: boolean;
}

const PictureModal: React.FC<PictureModalProps> = ({
  isOpen,
  onClose,
  picture,
  handlePrev,
  handleNext,
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
          <span>
            <h4
              className={styles.title}
            >{`${picture?.name} (${picture?.yearReleased})`}</h4>
            {picture?.isFavorite ? (
              <div className={styles.iconContainer}>
                <img className={styles.heartIcon} src={heartIcon} />
              </div>
            ) : null}
          </span>
          <p
            className={styles.modalText}
          >{`${picture?.cinema.name}, ${picture?.cinema.city}`}</p>
        </div>
      </div>
      {prev ? (
        <button
          className={`${styles.arrowButton} ${styles.left}`}
          onClick={handlePrev}
        >
          &lt;
        </button>
      ) : null}
      {next ? (
        <button
          className={`${styles.arrowButton} ${styles.right}`}
          onClick={handleNext}
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
