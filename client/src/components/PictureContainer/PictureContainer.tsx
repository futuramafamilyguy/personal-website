import usePictures from "../..//hooks/usePictures";
import MessageDisplay from "../MessageDisplay/MessageDisplay";
import MediaCard from "../MediaCard/MediaCard";
import Pagination from "../Pagination/Pagination";
import styles from "./PictureContainer.module.css";
import megamind from "../../assets/megamind.png";

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
                    "https://beam-images.warnermediacdn.com/BEAM_LWM_DELIVERABLES/64d42ace-e41a-42da-9370-a6623fa7471b/11312ad4-f5ca-417a-a411-87422e0c4f5e?host=wbd-images.prod-vod.h264.io&partner=beamcom"
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
