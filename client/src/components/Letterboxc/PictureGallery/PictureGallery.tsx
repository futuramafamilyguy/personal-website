import { useState } from "react";

import { useAuth } from "../../../contexts/AuthContext";
import usePicturesV2 from "../../../hooks/usePicturesV2";
import Picture from "../../../types/Picture";
import MessageDisplay from "../../MessageDisplay/MessageDisplay";
import PictureModal from "../PictureModal/PictureModal";
import PictureMonthRow from "../PictureMonthRow/PictureMonthRow";
import styles from "./PictureGallery.module.css";
import FavoritePicturesRow from "../FavoritePicturesRow.module.css/FavoritePicturesRow";
import { useYear } from "../../../contexts/YearContext";

/// <reference types="vite-plugin-svgr/client" />

const PictureGallery: React.FC = () => {
  const { pictures, loading, setTrigger } = usePicturesV2();

  const [selectedPicture, setSelectedPicture] = useState<Picture | null>(null);

  const isLoggedIn = useAuth();
  const year = useYear();
  const [modalOpen, setModalOpen] = useState(false);
  const [pictureIndex, setPictureIndex] = useState<number | null>(null);
  const [newModalOpen, setNewModalOpen] = useState(false);

  const openModal = (picture: Picture) => {
    setSelectedPicture(picture);
    setPictureIndex(pictures.indexOf(picture));
    setModalOpen(true);
  };

  const closeModal = () => {
    setSelectedPicture(null);
    setPictureIndex(null);
    setModalOpen(false);
  };

  const handlePrevPic = () => {
    if (pictureIndex !== null) {
      setSelectedPicture(pictures[pictureIndex - 1]);
      setPictureIndex(pictureIndex - 1);
    }
  };

  const handleNextPic = () => {
    if (pictureIndex !== null) {
      setSelectedPicture(pictures[pictureIndex + 1]);
      setPictureIndex(pictureIndex + 1);
    }
  };

  const openNewPictureModal = (index: number | null) => {
    setPictureIndex(index);
    setNewModalOpen(true);
  };

  const months: string[] = [
    "December",
    "November",
    "October",
    "September",
    "August",
    "July",
    "June",
    "May",
    "April",
    "March",
    "February",
    "January",
  ];

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      return (
        <div className={styles.pictureGallery}>
          {/* <div className={styles.pictureContainer}>
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
              <PictureModal
                isOpen={modalOpen}
                onClose={closeModal}
                picture={pictureIndex !== null ? pictures[pictureIndex] : null}
                handlePrev={handlePrevPic}
                handleNext={handleNextPic}
                prev={pictureIndex !== 0}
                next={pictureIndex !== pictures.length - 1}
              ></PictureModal>
            </div> */}
          <FavoritePicturesRow
            pictures={pictures.filter((p) => p.isFavorite === true)}
            year={year}
            pictureOnClick={(p: Picture) => openModal(p)}
            pictureEditable={isLoggedIn}
            pictureOnClickEdit={() => {}}
          />
          {months.map((month, index) => {
            var currentDate = new Date();
            var currentMonth = 12;
            if (year === currentDate.getFullYear()) {
              currentMonth = currentDate.getMonth() + 1;
              if (12 - index > currentMonth) {
                return null;
              }
            }

            return (
              <PictureMonthRow
                pictures={pictures.filter((p) => p.monthWatched === 12 - index)}
                month={month}
                pictureOnClick={(p: Picture) => openModal(p)}
                pictureEditable={isLoggedIn}
                pictureOnClickEdit={() => openNewPictureModal(index)}
              />
            );
          })}
          <PictureModal
            isOpen={modalOpen}
            onClose={closeModal}
            picture={selectedPicture}
            handlePrev={handlePrevPic}
            handleNext={handleNextPic}
            prev={pictureIndex !== 0}
            next={pictureIndex !== pictures.length - 1}
          />
        </div>
      );
    }
  };

  return renderContent();
};

export default PictureGallery;
