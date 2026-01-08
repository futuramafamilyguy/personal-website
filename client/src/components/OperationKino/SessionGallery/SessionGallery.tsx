import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";

import useSessions from "../../../hooks/useSessions";
import { Session } from "../../../types/Session";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import SessionModal from "../SessionModal/SessionModal";
import SessionRow from "../SessionRow/SessionRow";
import styles from "./SessionGallery.module.css";

const SessionGallery: React.FC = () => {
  const { sessions, loading } = useSessions();

  const [selectedSession, setSelectedSession] = useState<Session | null>(null);
  const [sessionIndex, setSessionIndex] = useState<number | null>(null);
  const location = useLocation();
  const modalOpen =
    location.pathname.includes("/focus") && selectedSession !== null;

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

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
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
