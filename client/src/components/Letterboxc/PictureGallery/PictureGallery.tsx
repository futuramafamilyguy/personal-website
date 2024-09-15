import { useState } from "react";

import { useAuth } from "../../../contexts/AuthContext";
import { useYear } from "../../../contexts/YearContext";
import usePicturesV2 from "../../../hooks/usePicturesV2";
import Picture from "../../../types/Picture";
import MessageDisplay from "../../MessageDisplay/MessageDisplay";
import NewMediaCard from "../../NewMediaCard/NewMediaCard";
import NewPictureModal from "../../NewPictureModal/NewPictureModal";
import FavoritePicturesRow from "../FavoritePicturesRow.module.css/FavoritePicturesRow";
import PictureModal from "../PictureModal/PictureModal";
import PictureMonthRow from "../PictureMonthRow/PictureMonthRow";
import styles from "./PictureGallery.module.css";

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

  const openNewPictureModal = (picture: Picture | null) => {
    setSelectedPicture(picture);
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
          {isLoggedIn ? (
            <>
              <hr />
              <div className={styles.newPictureRow}>
                <NewMediaCard
                  mediaType="Picture"
                  onClick={() => openNewPictureModal(null)}
                />
              </div>
            </>
          ) : null}
          <FavoritePicturesRow
            pictures={pictures.filter((p) => p.isFavorite === true)}
            year={year}
            pictureOnClick={(p: Picture) => openModal(p)}
            pictureEditable={isLoggedIn}
            pictureOnClickEdit={(p: Picture) => openNewPictureModal(p)}
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
                pictureOnClickEdit={(p: Picture) => openNewPictureModal(p)}
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
          {isLoggedIn ? (
            <NewPictureModal
              isOpen={newModalOpen}
              onClose={() => {
                setNewModalOpen(false);
                setSelectedPicture(null);
              }}
              picture={selectedPicture}
              setTrigger={() => setTrigger((prevTrigger) => !prevTrigger)}
            />
          ) : null}
        </div>
      );
    }
  };

  return renderContent();
};

export default PictureGallery;
