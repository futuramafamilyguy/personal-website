import usePictures from "../..//hooks/usePictures";
import Loading from "../Loading/Loading";
import MediaCard from "../MediaCard/MediaCard";
import Pagination from "../Pagination/Pagination";
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

  return (
    <>
      {loading ? (
        <Loading />
      ) : (
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
      )}
    </>
  );
};

export default PictureContainer;
