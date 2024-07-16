import styles from "./SessionModal.module.css";
import React from "react";
import ReactDom from "react-dom";

import { Session } from "../../types/Session";

interface SessionModalProps {
  isOpen: boolean;
  onClose: () => void;
  session: Session | null;
  handlePrev: () => void;
  handleNext: () => void;
  prev: boolean;
  next: boolean;
}

const SessionModal: React.FC<SessionModalProps> = ({
  isOpen,
  onClose,
  session,
  handlePrev,
  handleNext,
  prev,
  next,
}) => {
  const formatDate = (date: Date): string => {
    const options: Intl.DateTimeFormatOptions = {
      day: "2-digit",
      month: "short",
    };
    return new Date(date).toLocaleDateString("en-US", options);
  };

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.imageContainer}>
          <img
            src={session?.movie.imageUrl}
            alt={session?.movie.name}
            className={styles.modalImage}
          />
        </div>
        <div className={styles.modalTextBox}>
          <h4
            className={styles.title}
          >{`${session?.movie.name} (${session?.movie.yearReleased})`}</h4>
          <br />
          <h5>Showtimes</h5>
          <p>{session?.showtimes.map(formatDate).join(" | ")}</p>
          <br />
          <h5>Playing at these cinemas</h5>
          <ul>
            {session?.cinemas.map((cinema) => (
              <li>
                <a href={cinema.homePageUrl}>{cinema.name}</a>
              </li>
            ))}
          </ul>
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
      ;
    </>,
    document.getElementById("portal")!
  );
};

export default SessionModal;
