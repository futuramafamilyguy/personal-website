import React, { useState } from "react";
import ReactDom from "react-dom";

import heart from "../../../assets//svg/heart.png";
import flower from "../../../assets/motifs/flower.svg";
import Movie from "../../../types/Movie";
import styles from "./MovieModal.module.css";

interface MovieModalProps {
  isOpen: boolean;
  onClose: () => void;
  movie: Movie | null;
  handlePrev: () => void;
  handleNext: () => void;
  prev: boolean;
  next: boolean;
  isAltImage: boolean;
}

const icons: Record<string, string> = {
  flower,
  heart,
};

const MovieModal: React.FC<MovieModalProps> = ({
  isOpen,
  onClose,
  movie,
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
              isAltImage && movie?.altImageUrl
                ? movie?.altImageUrl
                : movie?.imageUrl
            }
            alt="where imag"
            className={styles.modalImage}
          />
        </div>
        <div
          className={
            isHovered ? styles.modalTextBox : styles.modalTextBoxHidden
          }
          style={{
            backgroundColor: movie?.isKino
              ? "rgba(227, 191, 70, 0.8)"
              : "rgba(255, 255, 255, 0.8)",
          }}
        >
          <span>
            <h4
              className={styles.title}
            >{`${movie?.name} (${movie?.releaseYear})`}</h4>
            {movie?.isNominated ? (
              <div className={styles.iconContainer}>
                <img
                  className={styles.favouriteIcon}
                  src={icons[movie?.motif] || icons["heart"]}
                />
              </div>
            ) : null}
          </span>
          <p
            className={styles.modalText}
          >{`${movie?.cinema.name}, ${movie?.cinema.city}`}</p>
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
      {movie?.zinger ? (
        <div className={styles.zinger}>{`"${movie?.zinger}"`}</div>
      ) : null}
      ;
    </>,
    document.getElementById("portal")!
  );
};

export default MovieModal;
