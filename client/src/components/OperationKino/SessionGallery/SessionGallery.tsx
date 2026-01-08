import { useEffect, useRef, useState } from "react";

import useSessions from "../../../hooks/useSessions";
import { Cinema, Session } from "../../../types/Session";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import SessionModal from "../SessionModal/SessionModal";
import SessionRow from "../SessionRow/SessionRow";
import styles from "./SessionGallery.module.css";
import { useIsMobile } from "../../../hooks/useIsMobile";
import { useNavigate } from "react-router-dom";

const SessionGallery: React.FC = () => {
  const { sessions, loading } = useSessions();

  const modalOpen = window.location.pathname.includes("/focus");
  const [selectedSession, setSelectedSession] = useState<Session | null>(null);
  const [sessionIndex, setSessionIndex] = useState<number | null>(null);

  const [newReleases, setNewReleases] = useState<Session[]>([]);
  const [retroClassics, setRetroClassics] = useState<Session[]>([]);
  const [isNewReleaseSelected, setIsNewReleaseSelected] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    setNewReleases(
      sessions.filter((s) => s.releaseYear >= new Date().getFullYear() - 1)
    );
    setRetroClassics(
      sessions.filter((s) => s.releaseYear < new Date().getFullYear() - 1)
    );
  }, [sessions]);

  const openModal = (session: Session, newRelease: boolean) => {
    setSelectedSession(session);
    setIsNewReleaseSelected(newRelease);
    if (newRelease) {
      setSessionIndex(newReleases.indexOf(session));
    } else {
      setSessionIndex(retroClassics.indexOf(session));
    }

    navigate("/operation-kino/focus");
  };

  const closeModal = () => {
    setSelectedSession(null);
    setSessionIndex(null);
    navigate("/operation-kino");
  };

  const handlePrevSession = () => {
    if (sessionIndex !== null) {
      if (isNewReleaseSelected) {
        setSelectedSession(newReleases[sessionIndex - 1]);
        setSessionIndex(sessionIndex - 1);
      } else {
        var length = newReleases.length;
        if (sessionIndex === 0 && length > 0) {
          setIsNewReleaseSelected(true);
          setSelectedSession(newReleases[length - 1]);
          setSessionIndex(length - 1);
        } else {
          setSelectedSession(retroClassics[sessionIndex - 1]);
          setSessionIndex(sessionIndex - 1);
        }
      }
    }
  };

  const handleNextSession = () => {
    if (sessionIndex !== null) {
      if (isNewReleaseSelected) {
        var length = retroClassics.length;
        if (sessionIndex === newReleases.length - 1 && length > 0) {
          setIsNewReleaseSelected(false);
          setSelectedSession(retroClassics[length - 1]);
          setSessionIndex(0);
        } else {
          setSelectedSession(newReleases[sessionIndex + 1]);
          setSessionIndex(sessionIndex + 1);
        }
      } else {
        setSelectedSession(retroClassics[sessionIndex + 1]);
        setSessionIndex(sessionIndex + 1);
      }
    }
  };

  const isMobile = useIsMobile();

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

  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollIntoView({
        behavior: "auto", // or "smooth" if you want animation
        block: "center", // vertically center the selected movie
      });
    }
  }, [selectedSession]);

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
    } else if (isMobile && modalOpen) {
      return (
        <div className={styles.mobileMovieList}>
          <div className={styles.mobileMovieHeader}>
            <span className={styles.headerTitle}>
              {isNewReleaseSelected ? "new releases" : "retro classics"}
            </span>

            <span className={styles.backButton} onClick={closeModal}>
              back
            </span>
          </div>
          {(isNewReleaseSelected ? newReleases : retroClassics).map(
            (session) => (
              <div
                key={session.title}
                className={styles.mobileMovieDetails}
                ref={
                  session.title === selectedSession?.title ? scrollRef : null
                } // <-- attach ref
              >
                <div className={styles.imageContainer}>
                  <img
                    src={session.imageUrl}
                    alt={session.title}
                    className={styles.modalImage}
                    loading="lazy"
                  />
                </div>

                <div className={styles.textBlock}>
                  <div className={styles.titleRow}>
                    <h5 className={styles.movieTitle}>
                      {session.title} ({session.releaseYear})
                    </h5>
                  </div>

                  <div className={styles.dateContainer}>
                    {session?.showtimes.map((showtime, index) => (
                      <div className={styles.dateBox} key={index}>
                        {formatDate(showtime)}
                      </div>
                    ))}
                  </div>
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
            )
          )}
        </div>
      );
    } else {
      return (
        <>
          <div className={styles.sessionGallery}>
            <SessionRow
              sessions={newReleases}
              category="new releases"
              sessionOnClick={(s: Session) => openModal(s, true)}
            />
            <SessionRow
              sessions={retroClassics}
              category="retro classics"
              sessionOnClick={(s: Session) => openModal(s, false)}
            />
            <SessionModal
              isOpen={modalOpen}
              onClose={closeModal}
              session={selectedSession}
              handlePrev={handlePrevSession}
              handleNext={handleNextSession}
              prev={
                sessionIndex !== 0 ||
                (!isNewReleaseSelected && newReleases.length > 0)
              }
              next={
                (isNewReleaseSelected &&
                  (sessionIndex !== newReleases.length - 1 ||
                    retroClassics.length > 0)) ||
                (!isNewReleaseSelected &&
                  sessionIndex !== retroClassics.length - 1)
              }
            ></SessionModal>
          </div>
        </>
      );
    }
  };

  return renderContent();
};

export default SessionGallery;
