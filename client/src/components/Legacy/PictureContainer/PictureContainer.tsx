import { useEffect, useState } from "react";

import { useAuth } from "../../../contexts/AuthContext";
import usePictures from "../../../hooks/usePictures";
import MediaCard from "../../Common/MediaCard/MediaCard";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import NewMediaCard from "../../Letterboxdc/NewMediaCard/NewMediaCard";
import NewPictureModal from "../../Letterboxdc/CreateMovieModal/CreateMovieModal";
import PictureModal from "../../Letterboxdc/MovieModal/MovieModal";
import Pagination from "../Pagination/Pagination";
import styles from "./PictureContainer.module.css";

const PictureContainer: React.FC = () => {
  const {
    movies,
    currentPage,
    totalPages,
    itemsPerPage,
    loading,
    emptyMediaCardArray,
    setCurrentPage,
    setTrigger,
  } = usePictures(1);

  const isLoggedIn = useAuth();
  const [modalOpen, setModalOpen] = useState(false);
  const [pictureIndex, setPictureIndex] = useState<number | null>(null);
  const [newModalOpen, setNewModalOpen] = useState(false);

  useEffect(() => {
    closeModal();
    setNewModalOpen(false);
  }, [totalPages]);

  const openModal = (index: number) => {
    setPictureIndex(index);
    setModalOpen(true);
  };

  const closeModal = () => {
    setPictureIndex(null);
    setModalOpen(false);
  };

  const handlePrevPic = () => {
    if (pictureIndex !== null) {
      if (pictureIndex === 0) {
        handlePrevPage();
        setPictureIndex(itemsPerPage - 1);
      } else {
        setPictureIndex((pictureIndex - 1 + movies.length) % movies.length);
      }
    }
  };

  const handleNextPic = () => {
    if (pictureIndex !== null) {
      if (pictureIndex === movies.length - 1) {
        handleNextPage();
        setPictureIndex(0);
      } else {
        setPictureIndex((pictureIndex + 1) % movies.length);
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

  const openNewPictureModal = (index: number | null) => {
    setPictureIndex(index);
    setNewModalOpen(true);
  };

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
    } else {
      if (isLoggedIn || movies.length > 0) {
        return (
          <>
            <div className={styles.pictureContainer}>
              {isLoggedIn ? (
                <NewMediaCard onClick={() => openNewPictureModal(null)} />
              ) : null}
              {isLoggedIn ? (
                <NewPictureModal
                  isOpen={newModalOpen}
                  onClose={() => setNewModalOpen(false)}
                  movie={pictureIndex !== null ? movies[pictureIndex] : null}
                  setTrigger={() => setTrigger((prevTrigger) => !prevTrigger)}
                />
              ) : null}

              {movies.map((movie, index) => (
                <MediaCard
                  key={movie.id}
                  imageUrl={movie.imageUrl}
                  title={movie.alias ?? movie.name}
                  highlighted={movie.isKino}
                  onClick={() => openModal(index)}
                  editable={isLoggedIn}
                  onClickEdit={() => openNewPictureModal(index)}
                />
              ))}
              {currentPage === totalPages &&
                emptyMediaCardArray.map((index) => (
                  <div className={styles.emptyMediaCard} key={index}></div>
                ))}

              <PictureModal
                isOpen={modalOpen}
                onClose={closeModal}
                movie={pictureIndex !== null ? movies[pictureIndex] : null}
                handlePrev={handlePrevPic}
                handleNext={handleNextPic}
                prev={pictureIndex !== 0 || currentPage !== 1}
                next={
                  pictureIndex !== movies.length - 1 ||
                  currentPage !== totalPages
                }
                isAltImage={false}
              ></PictureModal>
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
            message={"no pictures?"}
            imageUrl={
              "https://cdn.allenmaygibson.com/images/static/space-mom.jpg"
            }
          ></MessageDisplay>
        );
      }
    }
  };

  return renderContent();
};

export default PictureContainer;
