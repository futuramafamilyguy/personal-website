import { useEffect, useState } from "react";

import usePictures from "../..//hooks/usePictures";
import megamind from "../../assets/megamind.png";
import { useAuth } from "../../contexts/AuthContext";
import MediaCard from "../MediaCard/MediaCard";
import MessageDisplay from "../MessageDisplay/MessageDisplay";
import NewMediaCard from "../NewMediaCard/NewMediaCard";
import NewPictureModal from "../NewPictureModal/NewPictureModal";
import Pagination from "../Pagination/Pagination";
import PictureModal from "../Letterboxc/PictureModal/PictureModal";
import styles from "./PictureContainer.module.css";

const PictureContainer: React.FC = () => {
  const {
    pictures,
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
        setPictureIndex((pictureIndex - 1 + pictures.length) % pictures.length);
      }
    }
  };

  const handleNextPic = () => {
    if (pictureIndex !== null) {
      if (pictureIndex === pictures.length - 1) {
        handleNextPage();
        setPictureIndex(0);
      } else {
        setPictureIndex((pictureIndex + 1) % pictures.length);
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
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (isLoggedIn || pictures.length > 0) {
        return (
          <>
            <div className={styles.pictureContainer}>
              {isLoggedIn ? (
                <NewMediaCard
                  mediaType="Picture"
                  onClick={() => openNewPictureModal(null)}
                />
              ) : null}
              {isLoggedIn ? (
                <NewPictureModal
                  isOpen={newModalOpen}
                  onClose={() => setNewModalOpen(false)}
                  picture={
                    pictureIndex !== null ? pictures[pictureIndex] : null
                  }
                  setTrigger={() => setTrigger((prevTrigger) => !prevTrigger)}
                />
              ) : null}

              {pictures.map((picture, index) => (
                <MediaCard
                  key={picture.id}
                  imageUrl={picture.imageUrl}
                  title={picture.alias ?? picture.name}
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
                picture={pictureIndex !== null ? pictures[pictureIndex] : null}
                handlePrev={handlePrevPic}
                handleNext={handleNextPic}
                prev={pictureIndex !== 0 || currentPage !== 1}
                next={
                  pictureIndex !== pictures.length - 1 ||
                  currentPage !== totalPages
                }
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
            imageUrl={megamind}
          ></MessageDisplay>
        );
      }
    }
  };

  return renderContent();
};

export default PictureContainer;
