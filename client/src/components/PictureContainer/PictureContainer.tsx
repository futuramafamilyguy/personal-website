import { useState } from "react";

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
    loading,
    emptyMediaCardArray,
    setCurrentPage,
  } = usePictures(1);

  const [modalOpen, setModalOpen] = useState(false);
  const [pictureIndex, setPictureIndex] = useState<number | null>(null);

  const openModal = (index: number) => {
    setPictureIndex(index);
    setModalOpen(true);
  };

  const closeModal = () => {
    setPictureIndex(null);
    setModalOpen(false);
  };

  const handlePrev = () => {
    if (pictureIndex !== null) {
      setPictureIndex((pictureIndex - 1 + pictures.length) % pictures.length);
    }
  };

  const handleNext = () => {
    if (pictureIndex !== null) {
      setPictureIndex((pictureIndex + 1) % pictures.length);
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
                onPrev={handlePrev}
                onNext={handleNext}
                prev={pictureIndex !== 0}
                next={pictureIndex !== pictures.length - 1}
              ></PictureModal>
            </div>

            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={setCurrentPage}
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
