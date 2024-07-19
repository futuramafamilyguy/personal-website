import { useEffect, useState } from "react";

import usePictures from "../..//hooks/usePictures";
import megamind from "../../assets/megamind.png";
import MediaCard from "../MediaCard/MediaCard";
import MessageDisplay from "../MessageDisplay/MessageDisplay";
import Pagination from "../Pagination/Pagination";
import PictureModal from "../PictureModal/PictureModal";
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
  } = usePictures(1);

  const [modalOpen, setModalOpen] = useState(false);
  const [pictureIndex, setPictureIndex] = useState<number | null>(null);

  useEffect(() => {
    closeModal();
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

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (pictures.length > 0) {
        return (
          <>
            <div className={styles.pictureContainer}>
              {pictures.map((picture, index) => (
                <MediaCard
                  key={picture.id}
                  imageUrl={picture.imageUrl}
                  title={picture.alias ?? picture.name}
                  onClick={() => openModal(index)}
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
