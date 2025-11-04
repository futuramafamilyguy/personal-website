import React, { useEffect, useRef } from "react";
import ReactDom from "react-dom";

import { Cinema, Session } from "../../../types/Session";
import styles from "./SessionModal.module.css";

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

  const createCinemaComponent = (cinema: Cinema) => (
    <div key={cinema.name}>
      <a
        href={cinema.homepageUrl}
        className={styles.cinemaLink}
        target="_blank"
      >
        {cinema.name}
      </a>
    </div>
  );

  const textContainerRef = useRef<HTMLDivElement>(null);

  const handlePrevWrapper = () => {
    handlePrev();
    if (textContainerRef.current) {
      textContainerRef.current.scrollTop = 0;
    }
  };

  const handleNextWrapper = () => {
    handleNext();
    if (textContainerRef.current) {
      textContainerRef.current.scrollTop = 0;
    }
  };

  useEffect(() => {
    if (isOpen && textContainerRef.current) {
      textContainerRef.current.scrollTop = 0;
    }
  }, [isOpen]);

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.imageContainer}>
          <img
            src={session?.imageUrl}
            alt="where imag"
            className={styles.modalImage}
          />
        </div>
        <div className={styles.titleContainer}>
          <h4 className={styles.title}>{`${session?.title} ${
            session?.releaseYear === 0 ? "" : `(${session?.releaseYear})`
          }`}</h4>
        </div>
        <div
          className={`${styles.textContainer} bg-dark`}
          ref={textContainerRef}
        >
          <h5 className={styles.subtitle}>showtimes</h5>
          <div className={styles.dateContainer}>
            {session?.showtimes.map((showtime, index) => (
              <div className={styles.dateBox} key={index}>
                {formatDate(showtime)}
              </div>
            ))}
          </div>
          <hr />
          <h5 className={styles.subtitle}>playing at these cinemas</h5>
          <div className={styles.cinemaContainer}>
            <div className={styles.cinemaColumn}>
              {session?.cinemas
                .slice(0, Math.ceil(session?.cinemas.length / 2))
                .map(createCinemaComponent)}
            </div>
            <div className={styles.cinemaColumn}>
              {session?.cinemas
                .slice(Math.ceil(session?.cinemas.length / 2))
                .map(createCinemaComponent)}
            </div>
          </div>
        </div>
      </div>
      {prev ? (
        <button
          className={`${styles.arrowButton} ${styles.left}`}
          onClick={handlePrevWrapper}
        >
          &lt;
        </button>
      ) : null}
      {next ? (
        <button
          className={`${styles.arrowButton} ${styles.right}`}
          onClick={handleNextWrapper}
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
