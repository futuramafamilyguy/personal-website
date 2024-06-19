import usePictures from "../..//hooks/usePictures";
import MessageDisplay from "../MessageDisplay/MessageDisplay";
import MediaCard from "../MediaCard/MediaCard";
import Pagination from "../Pagination/Pagination";
import styles from "./PictureContainer.module.css";
import megamind from "../../assets/megamind.png";
import notFoundBoy from "../../assets/404boy.png";
import notFoundGirl from "../../assets/404girl.png";

const PictureContainer: React.FC = () => {
  const {
    pictures,
    currentPage,
    totalPages,
    loading,
    emptyMediaCardArray,
    setCurrentPage,
  } = usePictures(1);

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (pictures.length > 0) {
        return (
          <>
            <div className={styles.pictureContainer}>
              {pictures.map((picture) => (
                <MediaCard
                  key={picture.id}
                  imageUrl={
                    picture.imageUrl ??
                    (Math.random() < 0.5 ? notFoundBoy : notFoundGirl)
                  }
                  title={picture.alias ?? picture.name}
                />
              ))}
              {currentPage === totalPages &&
                emptyMediaCardArray.map((index) => (
                  <div className={styles.emptyMediaCard} key={index}></div>
                ))}
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
