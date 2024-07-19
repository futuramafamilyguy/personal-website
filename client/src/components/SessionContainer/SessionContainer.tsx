import { useEffect, useState } from "react";

import megamind from "../../assets/megamind.png";
import useSessions from "../../hooks/useSessions";
import MediaCard from "../MediaCard/MediaCard";
import MessageDisplay from "../MessageDisplay/MessageDisplay";
import Pagination from "../Pagination/Pagination";
import SessionModal from "../SessionModal/SessionModal";
import styles from "./SessionContainer.module.css";

const SessionContainer: React.FC = () => {
  const {
    currentSessions,
    currentPage,
    totalPages,
    itemsPerPage,
    loading,
    emptyMediaCardArray,
    setCurrentPage,
  } = useSessions(1);

  useEffect(() => {
    closeModal();
  }, [totalPages]);

  const [modalOpen, setModalOpen] = useState(false);
  const [sessionIndex, setSessionIndex] = useState<number | null>(null);

  const openModal = (index: number) => {
    setSessionIndex(index);
    setModalOpen(true);
  };

  const closeModal = () => {
    setSessionIndex(null);
    setModalOpen(false);
  };

  const handlePrevSession = () => {
    if (sessionIndex !== null) {
      if (sessionIndex === 0) {
        handlePrevPage();
        setSessionIndex(itemsPerPage - 1);
      } else {
        setSessionIndex(
          (sessionIndex - 1 + currentSessions.length) % currentSessions.length
        );
      }
    }
  };

  const handleNextSession = () => {
    if (sessionIndex !== null) {
      if (sessionIndex === currentSessions.length - 1) {
        handleNextPage();
        setSessionIndex(0);
      } else {
        setSessionIndex((sessionIndex + 1) % currentSessions.length);
      }
    }
  };

  const handlePrevPage = () => {
    if (currentPage > 1) {
      setCurrentPage(Math.max(currentPage - 1, 1));
    }
  };

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(Math.min(currentPage + 1, totalPages));
    }
  };

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (currentSessions.length > 0) {
        return (
          <>
            <div className={styles.sessionContainer}>
              {currentSessions.map((session, index) => (
                <MediaCard
                  key={index}
                  imageUrl={session.movie.imageUrl}
                  title={
                    session.movie.name.length > 21
                      ? session.movie.name.slice(0, 18) + "..."
                      : session.movie.name
                  }
                  onClick={() => openModal(index)}
                />
              ))}
              {currentPage === totalPages &&
                emptyMediaCardArray.map((index) => (
                  <div className={styles.emptyMediaCard} key={index}></div>
                ))}

              <SessionModal
                isOpen={modalOpen}
                onClose={closeModal}
                session={
                  sessionIndex !== null ? currentSessions[sessionIndex] : null
                }
                handlePrev={handlePrevSession}
                handleNext={handleNextSession}
                prev={sessionIndex !== 0 || currentPage !== 1}
                next={
                  sessionIndex !== currentSessions.length - 1 ||
                  currentPage !== totalPages
                }
              ></SessionModal>
            </div>

            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              handlePrev={handlePrevPage}
              handleNext={handleNextPage}
            />
          </>
        );
      } else {
        return (
          <MessageDisplay
            message={"no sessions?"}
            imageUrl={megamind}
          ></MessageDisplay>
        );
      }
    }
  };

  return renderContent();
};

export default SessionContainer;
