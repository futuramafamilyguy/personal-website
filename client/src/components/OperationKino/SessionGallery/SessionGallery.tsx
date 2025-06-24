import { useEffect, useState } from "react";

import useSessions from "../../../hooks/useSessions";
import { Session } from "../../../types/Session";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import SessionModal from "../SessionModal/SessionModal";
import SessionRow from "../SessionRow/SessionRow";
import styles from "./SessionGallery.module.css";

const SessionGallery: React.FC = () => {
  const { sessions, loading } = useSessions();

  const [modalOpen, setModalOpen] = useState(false);
  const [selectedSession, setSelectedSession] = useState<Session | null>(null);
  const [sessionIndex, setSessionIndex] = useState<number | null>(null);

  const [newReleaseSessions, setNewReleaseSessions] = useState<Session[]>([]);
  const [rereleaseSessions, setRereleaseSessions] = useState<Session[]>([]);
  const [isNewReleaseSelected, setIsNewReleaseSelected] = useState(false);

  useEffect(() => {
    setNewReleaseSessions(
      sessions.filter((s) => s.releaseYear === new Date().getFullYear())
    );
    setRereleaseSessions(
      sessions.filter((s) => s.releaseYear !== new Date().getFullYear())
    );
  }, [sessions]);

  const openModal = (session: Session, newRelease: boolean) => {
    setSelectedSession(session);
    setIsNewReleaseSelected(newRelease);
    if (newRelease) {
      setSessionIndex(newReleaseSessions.indexOf(session));
    } else {
      setSessionIndex(rereleaseSessions.indexOf(session));
    }

    setModalOpen(true);
  };

  const closeModal = () => {
    setSelectedSession(null);
    setSessionIndex(null);
    setModalOpen(false);
  };

  const handlePrevSession = () => {
    if (sessionIndex !== null) {
      if (isNewReleaseSelected) {
        setSelectedSession(newReleaseSessions[sessionIndex - 1]);
        setSessionIndex(sessionIndex - 1);
      } else {
        var length = newReleaseSessions.length;
        if (sessionIndex === 0 && length > 0) {
          setIsNewReleaseSelected(true);
          setSelectedSession(newReleaseSessions[length - 1]);
          setSessionIndex(length - 1);
        } else {
          setSelectedSession(rereleaseSessions[sessionIndex - 1]);
          setSessionIndex(sessionIndex - 1);
        }
      }
    }
  };

  const handleNextSession = () => {
    if (sessionIndex !== null) {
      if (isNewReleaseSelected) {
        var length = rereleaseSessions.length;
        if (sessionIndex === newReleaseSessions.length - 1 && length > 0) {
          setIsNewReleaseSelected(false);
          setSelectedSession(rereleaseSessions[length - 1]);
          setSessionIndex(0);
        } else {
          setSelectedSession(newReleaseSessions[sessionIndex + 1]);
          setSessionIndex(sessionIndex + 1);
        }
      } else {
        setSelectedSession(rereleaseSessions[sessionIndex + 1]);
        setSessionIndex(sessionIndex + 1);
      }
    }
  };

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
    } else {
      return (
        <>
          <div className={styles.sessionGallery}>
            <SessionRow
              sessions={newReleaseSessions}
              category="new releases"
              sessionOnClick={(s: Session) => openModal(s, true)}
            />
            <SessionRow
              sessions={rereleaseSessions}
              category="re-releases"
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
                (!isNewReleaseSelected && newReleaseSessions.length > 0)
              }
              next={
                (isNewReleaseSelected &&
                  (sessionIndex !== newReleaseSessions.length - 1 ||
                    rereleaseSessions.length > 0)) ||
                (!isNewReleaseSelected &&
                  sessionIndex !== rereleaseSessions.length - 1)
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
