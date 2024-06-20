import styles from "./ComingSoonPage.module.css";

const ComingSoonPage: React.FC = () => (
  <div className={styles.comingSoonPage}>
    <div className={styles.descriptionBox}>
      <h4>Coming Soon</h4>
      <br />
      <p>
        Following the industry standards I have decided to release the website
        incomplete without all the features I promised. They will eventually be
        implemented making this the biggest comeback in the industry.
      </p>
      <p>
        In the meantime, keep an eye on this page for what's to come or check
        out the{" "}
        <a
          href="https://github.com/futuramafamilyguy/personal-website"
          target="_blank"
          className={styles.externalLink}
        >
          GitHub repo
        </a>{" "}
        to view my progress. If I've been diligent, this page should be empty
        and you are most welcome to enjoy this picture of Lady Bird thrifting
        with her mum in Sacramento.
      </p>
    </div>
  </div>
);

export default ComingSoonPage;
