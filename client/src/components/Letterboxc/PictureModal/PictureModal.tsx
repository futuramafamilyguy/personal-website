import React, { useState } from "react";
import ReactDom from "react-dom";

import heartIcon from "../../../assets/svg/heart_filled.png";
import Picture from "../../../types/Picture";
import styles from "./PictureModal.module.css";

interface PictureModalProps {
  isOpen: boolean;
  onClose: () => void;
  picture: Picture | null;
  handlePrev: () => void;
  handleNext: () => void;
  prev: boolean;
  next: boolean;
  isAltImage: boolean;
}

const PictureModal: React.FC<PictureModalProps> = ({
  isOpen,
  onClose,
  picture,
  handlePrev,
  handleNext,
  prev,
  next,
  isAltImage,
}) => {
  const [isHovered, setIsHovered] = useState(false);

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div
        className={styles.modal}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        <div className={styles.imageContainer}>
          <img
            src={
              isAltImage && picture?.altImageUrl
                ? picture?.altImageUrl
                : picture?.imageUrl
            }
            alt={picture?.alias}
            className={styles.modalImage}
          />
        </div>
        <div
          className={
            isHovered ? styles.modalTextBox : styles.modalTextBoxHidden
          }
          style={{
            backgroundColor: picture?.isKino
              ? "rgba(227, 191, 70, 0.8)"
              : "rgba(255, 255, 255, 0.8)",
          }}
        >
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
          onMouseEnter={() => setIsHovered(true)}
          onMouseLeave={() => setIsHovered(false)}
        >
          &lt;
        </button>
      ) : null}
      {next ? (
        <button
          className={`${styles.arrowButton} ${styles.right}`}
          onClick={handleNext}
          onMouseEnter={() => setIsHovered(true)}
          onMouseLeave={() => setIsHovered(false)}
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
